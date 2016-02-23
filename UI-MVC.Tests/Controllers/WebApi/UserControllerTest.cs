using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.UI.Web.MVC.Controllers.Web_API;
using BB.BL.Domain;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using BB.UI.Web.MVC.Tests.Helpers;

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController controller;

        [TestInitialize]
        public void TestInitialize() {
            controller = new UserController(ContextEnum.BeatBuddyTest);
            DbInitializer.Initialize();
        }

        [TestMethod]
        public void GetUser()
        {
            
        }
    }
}
