using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Models;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Web;
using System.IO;
using BB.UI.Web.MVC.Controllers.Utils;
using System.Configuration;
using System.Net;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace BB.UI.Web.MVC.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistManager playlistManager;
        private readonly ITrackProvider trackProvider;
        private readonly IAlbumArtProvider albumArtProvider;
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;
        
        private const string testName = "jonah@gmail.com";

        User user = new User()
        {
            FirstName = "Jonah"
        };


        public PlaylistController(IPlaylistManager playlistManager, ITrackProvider trackProvider, IUserManager userManager, IOrganisationManager organisationManager, IAlbumArtProvider albumArtProvider)
        {
            this.playlistManager = playlistManager;
            this.trackProvider = trackProvider;
            this.userManager = userManager;
            this.organisationManager = organisationManager;
            this.albumArtProvider = albumArtProvider;
        }
        

        public ActionResult View(string key)
        {
            if (User != null)
            {
                user = userManager.ReadUser(User.Identity.Name);
            }
            var playlist = playlistManager.ReadPlaylistByKey(key);
            var votesUser = playlistManager.ReadVotesForUser(user);
            var organisation = organisationManager.ReadOrganisationForPlaylist(playlist.Id);

            var playlistOwners = new List<User>();
            if (organisation != null)
            {
                playlistOwners = userManager.ReadCoOrganiserFromOrganisation(organisation).ToList();
                playlistOwners.Add(userManager.ReadOrganiserFromOrganisation(organisation));
            }
            else
            {
                if (playlist.CreatedById != null)
                {
                    playlistOwners.Add(userManager.ReadUser((long)playlist.CreatedById));
                }
            }
            ViewBag.Organisation = organisationManager.ReadOrganisationForPlaylist(playlist.Id);
            ViewBag.CurrentUser = user;
            ViewBag.Organisers = playlistOwners;
            ViewBag.VotesUser = votesUser;
            ViewBag.PlaylistId = playlist.Id;
            ViewBag.PlaylistKey = playlist.Key;
//            ViewBag.CreatedBy = userManager.ReadUser((long)playlist.CreatedById);
            
            playlist.PlaylistTracks = playlist.PlaylistTracks.Where(t => t.PlayedAt == null).ToList();
            
            return View(playlist);
        }

        [HttpPost]
        public ActionResult AddVote(int vote, long id)
        {
            var user = userManager.ReadUser(User != null ? User.Identity.Name : testName);
            var createdVote = playlistManager.CreateVote(vote, user.Id, id);
            if (createdVote == null) return new HttpStatusCodeResult(400, "You have reached your vote limit for this playlist");

            var playlistTrack = playlistManager.ReadPlaylistTrack(id);
            var playlistId = playlistTrack.Playlist.Id;

            var viewmodel = new LivePlaylistTrackViewModel
            {
                Id = playlistTrack.Id,
                Score = playlistTrack.Votes.Sum(v => v.Score),
                Track = playlistTrack.Track
            };

            var context = GlobalHost.ConnectionManager.GetHubContext<PlaylistHub>();
            context.Clients.Group(playlistId.ToString()).scoreUpdated(id, viewmodel);

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult UnVote(long id)
        {
            var user = userManager.ReadUser(User != null ? User.Identity.Name : testName);
            playlistManager.DeleteVote(id, user.Id);

            var playlistTrack = playlistManager.ReadPlaylistTrack(id);
            var playlistId = playlistTrack.Playlist.Id;

            var viewmodel = new LivePlaylistTrackViewModel
            {
                Id = playlistTrack.Id,
                Score = playlistTrack.Votes.Sum(v => v.Score),
                Track = playlistTrack.Track
            };

            var context = GlobalHost.ConnectionManager.GetHubContext<PlaylistHub>();
            context.Clients.Group(playlistId.ToString()).scoreUpdated(id, viewmodel);

            return new HttpStatusCodeResult(200);
        }

        public ActionResult AddTrack(long id)
        {
            ViewBag.PlaylistId = id; // TODO: remove
            return View("AddTrack");
        }

        [HttpPost]
        public ActionResult AddTrack(long playlistId, string id)
        {
            if (!ModelState.IsValid) return View("View");

            var track = trackProvider.LookupTrack(id);
            if (track == null) return new HttpStatusCodeResult(400);

            var albumArtUrl = albumArtProvider.Find(track.Artist + " " + track.Title);
            track.CoverArtUrl = albumArtUrl;

            track = playlistManager.AddTrackToPlaylist(
                playlistId,
                track
            );

            if (track == null) return new HttpStatusCodeResult(400, "You can not add a song that is already in the list");
            var trackCount = playlistManager.ReadPlaylist(playlistId).PlaylistTracks.Where(p=>p.PlayedAt == null).ToList().Count;

            return Json(trackCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchTrack(string q)
        {
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.Search(q, maxResults: 3);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignPlaylistMaster(long id, string userEmail)
        {
            try
            {
                var playlist = playlistManager.ReadPlaylist(id);
                playlistManager.UpdatePlaylist(playlist, userEmail);
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public JsonResult GetUpcoming(long id)
        {
            var resultArray = new List<object>();

            var playlistTracks = playlistManager.ReadPlaylist(id)
                .PlaylistTracks
                .OrderByDescending(p => p.Votes.Sum(v => v.Score))
                .Where(t => t.PlayedAt == null)
                .Take(3)
                .ToList();

            resultArray.AddRange(playlistTracks);
            resultArray.Add(playlistTracks.Select(p => p.Votes.Sum(v => v.Score)).ToList());

            var jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(resultArray, Formatting.Indented, jss);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNextTrack(long id)
        {
            var playlistTracks = playlistManager.ReadPlaylist(id).PlaylistTracks.OrderByDescending(p => p.Votes.Sum(v => v.Score))
                .Where(t => t.PlayedAt == null);

            if (!playlistTracks.Any()) return Json(null, JsonRequestBehavior.DenyGet);

            var track = playlistTracks.OrderByDescending(p=>p.Votes.Sum(v=>v.Score)).First(t => t.PlayedAt == null);
            var playingViewModel = new CurrentPlayingViewModel()
            {
                TrackId = track.Track.TrackSource.TrackId,
                Title = track.Track.Title,
                Artist = track.Track.Artist,
                NextTracks = playlistTracks.Where(p=>p.PlayedAt==null).ToList().Count(),
                CoverArtUrl = track.Track.CoverArtUrl
            };
             return Json(playingViewModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPlaylist(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            playlist.PlaylistTracks = playlist.PlaylistTracks.Where(t => t.PlayedAt == null).ToList();
        
            return PartialView("PlaylistTable", playlist);
        }

        public ActionResult GetPlaylistGrid(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            playlist.PlaylistTracks = playlist.PlaylistTracks.Where(t => t.PlayedAt == null).ToList();

            return PartialView("PlaylistGrid", playlist);
        }
        [HttpPost]
        public ActionResult MoveTrackToHistory(long id)
        {
            var tracks = playlistManager.ReadPlaylist(id).PlaylistTracks;
            if (tracks.Count == 0) return new HttpStatusCodeResult(400);
            var track = tracks.OrderByDescending(p => p.Votes.Sum(v => v.Score)).First(t => t.PlayedAt == null);
            playlistManager.MarkTrackAsPlayed(track.Id, id);

                return new HttpStatusCodeResult(200);
            }

        public ActionResult IsNameAvailable(string email)
        {
            return Json(userManager.ReadUsers().All(org => org.Email != email),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            var model = new PlaylistViewModel()
            {
                Name = playlist.Name,
                Description = playlist.Description,
                ImageUrl = playlist.ImageUrl,
                Key = playlist.Key,
                MaximumVotesPerUser = playlist.MaximumVotesPerUser
            };
            return PartialView(model);
        }

        // POST: Default/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, PlaylistViewModel model, HttpPostedFileBase avatarImage)
        {
            try
            {
                var playlist = playlistManager.ReadPlaylist(id);
                playlist.Name = model.Name;
                playlist.Description = model.Description;
                playlist.Key = model.Key;
                playlist.ImageUrl = model.ImageUrl;
                playlist.MaximumVotesPerUser = model.MaximumVotesPerUser;
                string path = null;

                if (avatarImage != null && avatarImage.ContentLength > 0)
                {
                    var bannerFileName = Path.GetFileName(avatarImage.FileName);
                    path = FileHelper.NextAvailableFilename(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PlaylistImgPath"]), bannerFileName));
                    avatarImage.SaveAs(path);
                    path = Path.GetFileName(path);
                }
                playlist.ImageUrl = path;

                playlistManager.UpdatePlaylist(playlist);

                return RedirectToAction("Portal", "Home");
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

        // GET: Playlists
        public ActionResult Index()
        {
            return View(playlistManager.ReadPlaylists());
        }

        [HttpPost]
        public JsonResult Keycode(string key)
        {
            var playlists = playlistManager.ReadPlaylists();
            Playlist playlist = playlists.FirstOrDefault(p => p.Key == key);
            return Json(playlist.Id);
        }

        // GET: Playlists/Create
        [System.Web.Mvc.Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            var user = userManager.ReadUser(User.Identity.Name);
            ViewBag.MyOrganisations = organisationManager.ReadOrganisationsForUser(user.Id);
            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "User, Admin")]
        public ActionResult Create(PlaylistViewModel viewModel, HttpPostedFileBase avatarImage)
        {
            Organisation org = null;
            Playlist playlist;
            string path = null;

            if(viewModel.Name == null || viewModel.Name == "" || viewModel.Name == " ")
            {
                ModelState.AddModelError("Name", "You need to fill in a name for your playlist");
                return View(viewModel);
            }

            var user = userManager.ReadUser(User != null ? User.Identity.Name : testName);

            if (avatarImage != null && avatarImage.ContentLength > 0)
            {
                var bannerFileName = Path.GetFileName(avatarImage.FileName);
                path = FileHelper.NextAvailableFilename(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PlaylistImgPath"]), bannerFileName));
                avatarImage.SaveAs(path);
                path = Path.GetFileName(path);
            }

            try {
                if (viewModel.OrganisationId != 0)
                {
                    playlist = playlistManager.CreatePlaylistForOrganisation(viewModel.Name, viewModel.Description, viewModel.Key, viewModel.MaximumVotesPerUser, true, path, user, viewModel.OrganisationId);
                }
                else
                {
                    playlist = playlistManager.CreatePlaylistForUser(viewModel.Name, viewModel.Description, viewModel.Key, viewModel.MaximumVotesPerUser, true, path, user);
                }
                return RedirectToAction("View", new { key = playlist.Key });
            }
            catch (System.Exception e)
            {
                ModelState.AddModelError("Key", "The key value is already in use");
                return View(viewModel);
            }
            

        }

        public ActionResult Dashboard(long playlistId)
        {
            if (User != null)
            {
                user = userManager.ReadUser(User.Identity.Name);
            }
            var playlist = playlistManager.ReadPlaylist(playlistId);
            
            /*
            ViewBag.Organisers = playlistOwners;
            ViewBag.VotesUser = votesUser;
          */
            return View(playlist);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "User, Admin")]
        public ActionResult Delete(long id)
        {
            var playlist = playlistManager.DeletePlaylist(id);
            if (playlist == null) return new HttpStatusCodeResult(400);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult AddPlaylist(long playlistId, string id)
        {
            var youtubeProvider = new YouTubeTrackProvider();

            var tracks = youtubeProvider.LookUpPlaylist(id);

            foreach (Track track in tracks)
            {
                playlistManager.AddTrackToPlaylist(playlistId, track);
            }

            return null;
        }

        public JsonResult SearchPlaylist(string q)
        {
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.SearchPlaylist(q);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }
    }
}