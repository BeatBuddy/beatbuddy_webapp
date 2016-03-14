using BB.BL.Domain;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using BB.BL;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFPlaylist;
using BB.DAL.EFUser;

namespace BB.UI.Web.MVC.Tests.Helpers
{
    public static class DbInitializer
    {
        public static void Initialize() {
            var migratorConfig = new Migrations.Configuration();
            migratorConfig.TargetDatabase = new DbConnectionInfo(ContextEnum.BeatBuddyTest.ToString());
            var dbMigrator = new DbMigrator(migratorConfig);
            dbMigrator.Update();
        }

        public static IUserManager CreateUserManager()
        {
            return new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
        }
        public static IOrganisationManager CreateOrganisationManager()
        {
            return new OrganisationManager(new OrganisationRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
        }
        public static IPlaylistManager CreatePlaylistManager()
        {
            return new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddyTest)), new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
        }
    }
}
