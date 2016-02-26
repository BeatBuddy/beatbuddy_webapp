using System.Web.Mvc;

namespace BB.UI.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoggedIn() {
            string UserId = HttpContext.User.Identity.Name;
            return View();
        }
    }
}