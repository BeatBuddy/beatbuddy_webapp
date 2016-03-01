using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Controllers;
using BB.UI.Web.MVC.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.UI.Web.MVC.Tests.Helpers;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class OrganisationsControllerTest
    {
        private OrganisationsController _organisationsController;
        private IUserManager userManager;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _organisationsController = new OrganisationsController(ContextEnum.BeatBuddyTest);
            userManager = new UserManager(ContextEnum.BeatBuddyTest);
            DbInitializer.Initialize();
        }

        [TestMethod]
        public void TestOrganisationsIndexView()
        {
            ViewResult result = _organisationsController.Index() as ViewResult;
            Assert.IsNotNull(result);
            var organisations = result.Model as List<OrganisationViewModel>;
            Assert.AreEqual(1, organisations.Count);
        }

        [TestMethod]
        public void TestOrganisationsDetailsView_Correct_id()
        {
            ViewResult viewResult = _organisationsController.Details(1,1) as ViewResult;
            var organisation = (OrganisationViewWithPlaylist) viewResult.ViewData.Model; 
            Assert.AreEqual("Jonah's Songs", organisation.Name);
            Assert.AreEqual("Details", viewResult.ViewName);
        }

        [TestMethod]
        public void TestOrganisationsDetailsView_Wrong_Id()
        {
            ViewResult viewResult = _organisationsController.Details(-1,1) as ViewResult;
            Assert.AreEqual("Error", viewResult.ViewName);
            Assert.IsTrue(viewResult.ViewData.Model == null);
        }

        [TestMethod]
        public void TestCreateOrganisationView()
        {
            OrganisationViewModel organisation = new OrganisationViewModel()
            {
                Name = "Maarten's Songs"
            };
            
            RedirectToRouteResult viewResult = (RedirectToRouteResult) _organisationsController.Create(organisation, null);
            Assert.AreEqual("Details/2", viewResult.RouteValues["action"]);
        }

        [TestMethod]
        public void TestAddCoOrganiser()
        {
            _organisationsController.AddCoOrganiser(1, "jonah@gmail.com");
            User user = userManager.ReadUser("jonah@gmail.com");
            var userRoles = userManager.ReadOrganisationsForUser(user.Id);
            Assert.IsNotNull(userRoles);
            Assert.AreEqual(userRoles.Count(),1);

        }

        [TestMethod]
        public void TestAddCoOrganiser_Fail()
        {
            HttpStatusCodeResult result = _organisationsController.AddCoOrganiser(1, "jonah@gmail.com") as HttpStatusCodeResult;
            Assert.AreEqual(result.StatusCode, 400);
        }


    }
}
