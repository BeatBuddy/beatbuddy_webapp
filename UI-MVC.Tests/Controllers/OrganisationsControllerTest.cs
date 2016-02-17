using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;

using BB.UI.Web.MVC.Controllers;
using BB.UI.Web.MVC.Models;
using BB.UI.Web.MVC.Migrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class OrganisationsControllerTest
    {
        private OrganisationsController _organisationsController;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _organisationsController = new OrganisationsController(ContextEnum.BeatBuddyTest);
            var migratorConfig = new Migrations.Configuration();
            migratorConfig.TargetDatabase = new DbConnectionInfo(ContextEnum.BeatBuddyTest.ToString());
            var dbMigrator = new DbMigrator(migratorConfig);
            dbMigrator.Update();
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
            ViewResult viewResult = _organisationsController.Details(1) as ViewResult;
            var organisation = (OrganisationViewModel) viewResult.ViewData.Model; 
            Assert.AreEqual("Jonah's Songs", organisation.Name);
            Assert.AreEqual("Details", viewResult.ViewName);
        }

        [TestMethod]
        public void TestOrganisationsDetailsView_Wrong_Id()
        {
            ViewResult viewResult = _organisationsController.Details(-1) as ViewResult;
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
            RedirectToRouteResult viewResult = (RedirectToRouteResult) _organisationsController.Create(organisation, null, null);
            Assert.AreEqual("Index", viewResult.RouteValues["action"]);
        }

        

    }
}
