namespace BB.UI.Web.MVC.Migrations
{
    using BL;
    using BL.Domain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<BB.UI.Web.MVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BB.UI.Web.MVC.Models.ApplicationDbContext context)
        {
            AddUserAndRole(context);
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            
        }

        bool AddUserAndRole(ApplicationDbContext context)
        {

            IdentityResult ir;
            IdentityResult ir1;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("Admin"));
            ir1 = rm.Create(new IdentityRole("User"));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "admin@admin.com",
            };
            var user1 = new ApplicationUser()
            {
                UserName = "user@user.com"
            };
            ir = um.Create(user, "password");
            ir1 = um.Create(user1, "password");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Admin");
            if (ir1.Succeeded == false)
                return ir1.Succeeded;
            ir1 = um.AddToRole(user1.Id, "User");
            var userManager = new UserManager(ContextEnum.BeatBuddyTest);
            userManager.CreateUser("admin@admin.com", "Heylen", "Matthias", "acidshards", "/");
            return ir.Succeeded;
        }
    }
}
