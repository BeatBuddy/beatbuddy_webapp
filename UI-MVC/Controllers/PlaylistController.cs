using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Models;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using System.Web;
using System.IO;
using BB.UI.Web.MVC.Controllers.Utils;
using System.Configuration;
using System.Net;

namespace BB.UI.Web.MVC.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistManager playlistManager;
        private readonly ITrackProvider trackProvider;
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;

        
        private const string testName = "jonah@gmail.com";

        User user = new User()
        {
            FirstName = "Jonah"
        };


        public PlaylistController(IPlaylistManager playlistManager, ITrackProvider trackProvider, IUserManager userManager, IOrganisationManager organisationManager)
        {
            this.playlistManager = playlistManager;
            this.trackProvider = trackProvider;
            this.userManager = userManager;
            this.organisationManager = organisationManager;
        }
        
        

        public ActionResult View(long id)
        {
            if (User != null)
            {
                user = userManager.ReadUser(User.Identity.Name);
            }
            var playlist = playlistManager.ReadPlaylist(id);
            var votesUser = playlistManager.ReadVotesForUser(user);
            var organisation = organisationManager.ReadOrganisationForPlaylist(id);
            List<User> playlistOwners = new List<User>();
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
            ViewBag.Organisers = playlistOwners;
      
            ViewBag.VotesUser = votesUser;
            ViewBag.PlaylistId = id;
            

            
            playlist.PlaylistTracks = playlist.PlaylistTracks.Where(t => t.PlayedAt == null).ToList();
            
            return View(playlist);
        }
        

        [HttpPost]
        public ActionResult AddVote(int vote, long id)
        {
            var user = userManager.ReadUser(User != null ? User.Identity.Name : testName);
            playlistManager.CreateVote(vote, user.Id, id);
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

            track = playlistManager.AddTrackToPlaylist(
                playlistId,
                track
            );

            if (track == null) return new HttpStatusCodeResult(400, "You can not add a song that is already in the list");
            

            return new HttpStatusCodeResult(200);
        }

        public JsonResult SearchTrack(string q)
        {
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.Search(q, maxResults:3);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignPlaylistMaster(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            //playlist.
            //playlistManager.UpdatePlaylist()
            return null;
        }

        public ActionResult GetNextTrack(long id)
        {
            var playlistTracks = playlistManager.ReadPlaylist(id).PlaylistTracks
                .Where(t => t.PlayedAt == null);

            if (!playlistTracks.Any()) return Json(null, JsonRequestBehavior.DenyGet);

            var track = playlistTracks.First(t => t.PlayedAt == null);

                return Json(new
                {
                trackId = track.Track.TrackSource.TrackId,
                trackName = track.Track.Title,
                artist = track.Track.Artist,
                nextTracks = playlistTracks.Count(),
                thumbnail = track.Track.CoverArtUrl
                }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPlaylist(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            playlist.PlaylistTracks = playlist.PlaylistTracks.Where(t => t.PlayedAt == null).ToList();
        
            return PartialView("PlaylistTable", playlist);
        }

        [HttpPost]
        public ActionResult MoveTrackToHistory(long id)
        {
            var tracks = playlistManager.ReadPlaylist(id).PlaylistTracks;
            if (tracks.Count == 0) return new HttpStatusCodeResult(400);

            playlistManager.MarkTrackAsPlayed(
                playlistManager.ReadPlaylist(id).PlaylistTracks
                //.OrderByDescending(t => t.Score)
                .First(t => t.PlayedAt == null).Id);

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

        // GET: Playlists/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            var user = userManager.ReadUser(User.Identity.Name);
            ViewBag.UserOrganisations = organisationManager.ReadOrganisationsForUser(user.Id);
            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create(PlaylistViewModel viewModel, HttpPostedFileBase avatarImage)
        {
            Organisation org = null;
            Playlist playlist;
            string path = null;

            var user = userManager.ReadUser(User != null ? User.Identity.Name : testName);

            if (viewModel.OrganisationId != 0)
            {
                try
                {
                    org = organisationManager.ReadOrganisation(viewModel.OrganisationId);
                }
                catch
                {
                    ModelState.AddModelError("OrganisationFault", "The organisation could not be found or you have insufficient rights");
                    return View("Create");
                }
            }
            if (avatarImage != null && avatarImage.ContentLength > 0)
            {
                var bannerFileName = Path.GetFileName(avatarImage.FileName);
                path = FileHelper.NextAvailableFilename(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PlaylistImgPath"]), bannerFileName));
                avatarImage.SaveAs(path);
                path = Path.GetFileName(path);
            }

            if (org != null)
            {
                playlist = playlistManager.CreatePlaylistForOrganisation(viewModel.Name, viewModel.Description, viewModel.Key, viewModel.MaximumVotesPerUser, true, path, user, org);
            }
            else
            {
                playlist = playlistManager.CreatePlaylistForUser(viewModel.Name, viewModel.Description, viewModel.Key, viewModel.MaximumVotesPerUser, true, path, user);
            }
            
            

            return RedirectToAction("View/" + playlist.Id);

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(long id)
        {
            var playlist = playlistManager.DeletePlaylist(id);
            if (playlist == null) return new HttpStatusCodeResult(400);
            return new HttpStatusCodeResult(200);
        }
    }
}