using System.Linq;
using System.Net;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain.Playlists;

namespace BB.UI.Web.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly IPlaylistManager playlistManager;

        public CommentController(IPlaylistManager playlistManager)
        {
            this.playlistManager = playlistManager;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Comment comment, long id)
        {
            var username = User.Identity.Name;

            var createdComment = playlistManager.CreateComment(id, comment.Text, User.Identity.Name);

            if(createdComment != null)
                return Json(createdComment);
            else 
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        public ActionResult GetComments(long id)
        {
            var comments = playlistManager.ReadComments(new Playlist { Id = id }).OrderBy(c => c.TimeStamp).ToList();
            return PartialView("_CommentsPartial", comments);
        }
    }
}