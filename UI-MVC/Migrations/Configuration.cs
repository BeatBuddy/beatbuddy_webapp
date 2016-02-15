namespace BB.UI.Web.MVC.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BB.UI.Web.MVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BB.UI.Web.MVC.Models.ApplicationDbContext context)
        {
            AddRoles(context);
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

        bool AddRoles(ApplicationDbContext context)
        {
            
            
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            rm.Create(new IdentityRole("Admin"));
            rm.Create(new IdentityRole("User"));

            return true;
        }
    }
}
