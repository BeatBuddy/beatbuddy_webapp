using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        public ActionResult SearchOrganisation(string q)
        {
            List<Organisation> searchResult = organisationManager.SearchOrganisations(q).ToList();
            for(int i=0; i<searchResult.Count; i++)
            {
                if (searchResult[i].BannerUrl == null)
                {
                    searchResult[i].BannerUrl = "/Content/img/login-banner.jpg";
                }
                else {
                    searchResult[i].BannerUrl = "/Content/img/Organisations/" + searchResult[i].BannerUrl;
                }
            }
            string json =  JsonConvert.SerializeObject(searchResult, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
            return new ContentResult { Content = json, ContentType = "application/json" };
        }

    }
}