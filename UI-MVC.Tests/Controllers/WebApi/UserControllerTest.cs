using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.UI.Web.MVC.Controllers.Web_API;
using BB.BL.Domain;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController controller;

        [TestInitialize]
        public void TestInitialize() {
            controller = new UserController(ContextEnum.BeatBuddyTest);
            var migratorConfig = new Migrations.Configuration
            {
                TargetDatabase = new DbConnectionInfo(ContextEnum.BeatBuddyTest.ToString())
            };
            var dbMigrator = new DbMigrator(migratorConfig);
            dbMigrator.Update();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
