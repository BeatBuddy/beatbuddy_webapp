using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFPlaylist;
using BB.DAL.EFUser;
using BB.UI.Web.MVC.Controllers;
using BB.UI.Web.MVC.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            
            HomeController controller = new HomeController(DbInitializer.CreateUserManager(), DbInitializer.CreatePlaylistManager(),DbInitializer.CreateOrganisationManager());
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
