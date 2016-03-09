using System.Linq;
using System.Web.Mvc;
using BB.BL;

namespace BB.UI.Web.MVC.Controllers
{
    public class HistoryController : Controller
    {
        readonly IPlaylistManager playlistManager;

        public HistoryController(IPlaylistManager playlistManager)
        {
            this.playlistManager = playlistManager;
        }

        // GET: History/View/1
        public ActionResult View(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            playlist.PlaylistTracks = playlist.PlaylistTracks
                .Where(pt => pt.PlayedAt != null)
                .OrderBy(pt => pt.PlayedAt)
                .ToList();

            var comments = playlistManager.ReadComments(playlist);
            ViewBag.CommentCount = comments.Count();
            ViewBag.Comments = comments;
            
            return View(playlist);
        }
    }
}