namespace TaskilyWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;

    using TaskilyWeb.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TaskilyWeb.DAL.TasklyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaskilyWeb.DAL.TasklyDbContext context)
        {
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

            var RoleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!RoleManager.RoleExists("Admin"))
            {
                var role = RoleManager.Create(new IdentityRole("Admin"));
            }

            /*
             //  Local Installs only make a user admin at start up
             //
            if (context.Users.Any(u => u.UserName == "***********"))
            {
                var store = new UserStore<TasklyUser>(context);
                var manager = new UserManager<TasklyUser>(store);
                var user = context.Users.Where(u => u.UserName == "***********").FirstOrDefault();

                if ( user != null )
                {
                    if (!manager.IsInRole(user.Id, "Admin"))
                    {
                        manager.AddToRole(user.Id, "Admin");
                    }

                }
            }
            */


        }
    }
}
