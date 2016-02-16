using System.Web.Mvc;
using BB.BL;

namespace BB.UI.Web.MVC.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistManager playlistManager;
        private readonly ITrackProvider trackProvider;

        public PlaylistController(IPlaylistManager playlistManager, ITrackProvider trackProvider)
        {
            this.playlistManager = playlistManager;
            this.trackProvider = trackProvider;
        }

        public PlaylistController()
        {
            playlistManager = new PlaylistManager();
            trackProvider = new YouTubeTrackProvider();
        }

        public ActionResult View(long id)
        {
            return View();
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
    }
}