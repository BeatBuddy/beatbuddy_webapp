using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BB.BL.Domain;
using BB.UI.Web.MVC.Controllers;
using BB.UI.Web.MVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {

        private AccountController _accountController;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountController = new AccountController(ContextEnum.BeatBuddyTest);
            var migratorConfig = new Migrations.Configuration();
            migratorConfig.TargetDatabase = new DbConnectionInfo(ContextEnum.BeatBuddyTest.ToString());
            var dbMigrator = new DbMigrator(migratorConfig);
            dbMigrator.Update();
        }

        [TestMethod]
        public void TestRegister()
        {
            
        }
       
    }
}
