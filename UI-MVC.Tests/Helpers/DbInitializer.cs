using BB.BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.UI.Web.MVC.Tests.Helpers
{
    public static class DbInitializer
    {
        public static void Initialize() {
            var migratorConfig = new Migrations.Configuration();
            migratorConfig.TargetDatabase = new DbConnectionInfo(ContextEnum.BeatBuddyTest.ToString());
            var dbMigrator = new DbMigrator(migratorConfig);
            //dbMigrator.Update();
        }
    }
}
