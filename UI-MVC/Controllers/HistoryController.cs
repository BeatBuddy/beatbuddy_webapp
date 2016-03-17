using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain.Users;

namespace BB.UI.Web.MVC.Controllers
{
    public class HistoryController : Controller
    {
        readonly IPlaylistManager playlistManager;
        readonly IOrganisationManager organisationManager;
        readonly IUserManager userManager;

        public HistoryController(IPlaylistManager playlistManager, IOrganisationManager organisationManager, IUserManager userManager)
        {
            this.playlistManager = playlistManager;
            this.organisationManager = organisationManager;
            this.userManager = userManager;
        }

        // GET: History/View/1
        public ActionResult View(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            var comments = playlistManager.ReadComments(playlist);
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

            User user = null;
            if (User != null) user = userManager.ReadUser(User.Identity.Name);

            ViewBag.Organisation = organisationManager.ReadOrganisationForPlaylist(playlist.Id);
            ViewBag.CurrentUser = user;
            ViewBag.Organisers = playlistOwners;

            ViewBag.CommentCount = comments.Count();
            ViewBag.Comments = comments;
            ViewBag.TrackCount = playlist.PlaylistTracks.Count(pt => pt.PlayedAt == null);

            playlist.PlaylistTracks = playlist.PlaylistTracks
                .Where(pt => pt.PlayedAt != null)
                .OrderBy(pt => pt.PlayedAt)
                .ToList();

            return View(playlist);
        }
    }
}