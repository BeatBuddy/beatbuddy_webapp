using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;

namespace BB.UI.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManager userManager;
        private readonly IPlaylistManager playlistManager;
        private readonly IOrganisationManager organisationManager;

        public HomeController(ContextEnum contextEnum)
        {
            userManager = new UserManager(contextEnum);
            playlistManager = new PlaylistManager(contextEnum);
            organisationManager = new OrganisationManager(contextEnum);
        }

        public HomeController()
        {
            userManager = new UserManager(ContextEnum.BeatBuddy);
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddy);
            organisationManager = new OrganisationManager(ContextEnum.BeatBuddy);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoggedIn()
        {
            var user = userManager.ReadUser(HttpContext.User.Identity.Name);
            if (user == null) return RedirectToAction("Index", "Home");

            ViewBag.Name = user.FirstName;
            ViewBag.MyOrganisations = organisationManager.ReadOrganisations().Take(4).ToList(); //userManager.ReadOrganisationsForUser(user.Id).ToList();
            ViewBag.MyPlaylists = playlistManager.ReadPlaylists().Take(5).ToList(); //TODO: only fetch playlists of user
            ViewBag.RecommendedPlaylists = playlistManager.ReadPlaylists().Take(5).ToList();
            return View();
        }
    }
}