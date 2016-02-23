using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using BB.BL.Domain.Users;
using BB.DAL;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using BB.BL.Domain;
using BB.UI.Web.MVC.Migrations;

namespace BB.BL.Tests
{
    [TestClass]
    public class UserManagerUnitTest
    {
        private UserManager userManager;

        [TestInitialize]
        public void TestInitialize()
        {
            userManager = new UserManager(ContextEnum.BeatBuddyTest);
            var migratorConfig = new UI.Web.MVC.Migrations.Configuration();
            migratorConfig.TargetDatabase = new DbConnectionInfo(ContextEnum.BeatBuddyTest.ToString());
            var dbMigrator = new DbMigrator(migratorConfig);
            dbMigrator.Update();
        }

    }
}
