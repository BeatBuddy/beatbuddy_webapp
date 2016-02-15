using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class OrganisationsControllerTest
    {
        private OrganisationsController _organisationsController;
        private Mock<IOrganisationManager> _organisationManagerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _organisationManagerMock = new Mock<IOrganisationManager>();
            _organisationsController = new OrganisationsController(_organisationManagerMock.Object);
            var organisation = new Organisation()
            {
                Id = 1,
                Name = "Jonah's Songs"
            };
            var organisations = new List<Organisation>() {organisation};
            _organisationManagerMock.Setup(mgr => mgr.ReadOrganisation(1)).Returns(organisation);
            _organisationManagerMock.Setup(mgr => mgr.ReadOrganisations()).Returns(organisations);
            

        }

        [TestMethod]
        public void TestOrganisationsIndexView()
        {
            ViewResult result = _organisationsController.Index() as ViewResult;
            Assert.IsNotNull(result);
            var organisations = result.Model as List<Organisation>;
            Assert.AreEqual(organisations.Count, 1);
            Assert.AreEqual(organisations.First().Id, 1);
        }

        [TestMethod]
        public void TestOrganisationsDetailsView_Correct_id()
        {
            ViewResult viewResult = _organisationsController.Details(1) as ViewResult;
            var organisation = (Organisation) viewResult.ViewData.Model; 
            Assert.AreEqual("Jonah's Songs", organisation.Name);
            Assert.AreEqual(1, organisation.Id);
            Assert.AreEqual("Details", viewResult.ViewName);
        }

        [TestMethod]
        public void TestOrganisationsDetailsView_Wrong_Id()
        {
            ViewResult viewResult = _organisationsController.Details(100) as ViewResult;
            Assert.AreEqual("Error", viewResult.ViewName);
            Assert.IsTrue(viewResult.ViewData.Model == null);
        }

        [TestMethod]
        public void TestCreateOrganisationView()
        {
            Organisation organisation = new Organisation()
            {
                Name = "Jonah's Songs"
            };
            RedirectToRouteResult viewResult = (RedirectToRouteResult) _organisationsController.Create(organisation);
            
            Assert.AreEqual("Index", viewResult.RouteValues["action"]);
            //Assert.IsTrue(viewResult.ViewData.Model == null);
        }

    }
}
