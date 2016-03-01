using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;

namespace BB.UI.Web.MVC.Controllers
{
    public class HistoryController : Controller
    {
        readonly IPlaylistManager playlistManager;

        public HistoryController(ContextEnum contextEnum)
        {
            playlistManager = new PlaylistManager(contextEnum);
        }

        public HistoryController()
        {
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddy);
        }

        // GET: History/View/1
        public ActionResult View(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            playlist.PlaylistTracks = playlist.PlaylistTracks.Where(pt => pt.PlayedAt == null).ToList();
            
            return View(playlist);
        }
    }
}