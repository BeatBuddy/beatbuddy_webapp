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


        public HomeController(IUserManager userManager, IPlaylistManager playlistManager,
            IOrganisationManager organisationManager)
        {
            this.userManager = userManager;
            this.playlistManager = playlistManager;
            this.organisationManager = organisationManager;
        }
        

        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Portal()
        {
            var user = userManager.ReadUser(HttpContext.User.Identity.Name);
            if (user == null) return RedirectToAction("Index", "Home");

            ViewBag.Name = user.FirstName;
            ViewBag.MyOrganisations = organisationManager.ReadOrganisationsForUser(user.Id).ToList(); 
            ViewBag.MyPlaylists = playlistManager.ReadPlaylistsForUser(user.Id).ToList(); 
            ViewBag.RecommendedPlaylists = playlistManager.ReadPlaylists().Reverse().Take(3).ToList();
            return View();
        }
    }
}