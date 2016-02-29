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


        public PlaylistController(IPlaylistManager playlistManager, ITrackProvider trackProvider, UserManager userManager)
        {
            this.playlistManager = playlistManager;
            this.trackProvider = trackProvider;
            this.userManager = userManager;
        }

        public PlaylistController()
        {
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddy);
            userManager = new UserManager(ContextEnum.BeatBuddy);
            organisationManager = new OrganisationManager(ContextEnum.BeatBuddy);
            trackProvider = new YouTubeTrackProvider();
        }

        public PlaylistController(ContextEnum contextEnum)
        {
            playlistManager = new PlaylistManager(contextEnum);
            userManager = new UserManager(contextEnum);
            organisationManager = new OrganisationManager(contextEnum);
            trackProvider = new YouTubeTrackProvider();
        }

        public ActionResult View(long id)
        {
            if (User != null)
            {
                user = userManager.ReadUser(User.Identity.Name);
            }
            var votesUser = playlistManager.ReadVotesForUser(user);
            ViewBag.VotesUser = votesUser;
            var playlist = playlistManager.ReadPlaylist(id);
            ViewBag.PlaylistId = id;
            return View(playlist);
        }

        [HttpPost]
        public ActionResult AddVote(int vote, long id)
        {
            Vote v = new Vote();
            v.Score = vote;
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

            if(track == null) return new HttpStatusCodeResult(400);

            return new HttpStatusCodeResult(200);
        }


        public JsonResult SearchTrack(string q)
        {
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.Search(q);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNextTrack(long id)
        {
            var playlistTracks = playlistManager.ReadPlaylist(id).PlaylistTracks;
            if (playlistTracks.Count != 0)
            {
                return Json(new
                {
                    trackId = playlistTracks.First().Track.TrackSource.TrackId,
                    trackName = playlistTracks.First().Track.Title,
                    artist = playlistTracks.First().Track.Artist,
                    nextTracks = playlistTracks.Count()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.DenyGet);
        }

        public ActionResult GetPlaylist(long id)
        {
            return PartialView("PlaylistTable",playlistManager.ReadPlaylist(id));
        
        }



        [HttpPost]
        public ActionResult MoveTrackToHistory(long id)
        {
            var tracks = playlistManager.ReadPlaylist(id).PlaylistTracks;
            if (tracks.Count != 0)
            {
                playlistManager.DeletePlaylistTrack(playlistManager.ReadPlaylist(id).PlaylistTracks.First().Id);
                return new HttpStatusCodeResult(200);
            }
            else return new HttpStatusCodeResult(400);
        }
        public ActionResult IsNameAvailable(string email)
        {
            return Json(userManager.ReadUsers().All(org => org.Email!=email),
                JsonRequestBehavior.AllowGet);
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
            ViewBag.UserOrganisations = organisationManager.ReadOrganisations(user.Id);
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
    }
}