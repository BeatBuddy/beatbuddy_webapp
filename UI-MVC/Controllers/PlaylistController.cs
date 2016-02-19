using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Models;

namespace BB.UI.Web.MVC.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistManager playlistManager;
        private readonly ITrackProvider trackProvider;
        private readonly IUserManager userManager;

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
            trackProvider = new YouTubeTrackProvider();
        }

        public PlaylistController(ContextEnum contextEnum)
        {
            playlistManager = new PlaylistManager(contextEnum);
            userManager = new UserManager(contextEnum);
            trackProvider = new YouTubeTrackProvider();
        }
        public ActionResult View(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            return View(playlist);
        }

        public ActionResult AddTrack(long id)
        {
            ViewBag.PlaylistId = id; // TODO: remove
            return View();
        }

        [HttpPost]
        public ActionResult AddTrack(long playlistId, string id)
        {
            if (!ModelState.IsValid) return View("View");

            var track = trackProvider.LookupTrack(id);
            if (track == null) return new HttpStatusCodeResult(400);

            track = playlistManager.AddTrackToPlaylist(
                playlistId,
                track.Artist,
                track.Title,
                track.TrackSource,
                track.CoverArtUrl
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

        public ActionResult IsNameAvailable(string email)
        {
            return Json(userManager.ReadUsers().All(org => org.Email.Equals(email)),
                JsonRequestBehavior.AllowGet);
        }

        // GET: Playlists
        public ActionResult Index()
        {
            return View(playlistManager.ReadPlaylists());
        }

        // GET: Playlists/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Playlists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        public ActionResult Create(PlaylistViewModel collection)
        {
            string username = User.Identity.Name;
            // TODO: Add insert logic here
            User user = userManager.ReadUser(username);
            User playlistMaster = userManager.ReadUser(collection.PlaylistMaster);
            Playlist playlist = playlistManager.CreatePlaylistForUser(collection.Name, collection.MaximumVotesPerUser, true, collection.ImageUrl, playlistMaster, user);

            return RedirectToAction("Index");

        }
    }
}