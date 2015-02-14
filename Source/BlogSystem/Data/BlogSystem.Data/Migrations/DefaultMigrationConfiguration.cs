namespace BlogSystem.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using BlogSystem.Common;
    using BlogSystem.Data.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class DefaultMigrationConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DefaultMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.SeedRoles(context);
            this.SeedSettings(context);
        }

        protected void SeedSettings(ApplicationDbContext context)
        {
            if (context.Settings.Any())
            {
                return;
            }

            context.Settings.Add(new Setting { Name = "Logo URL", Value = "/img/header-logo.png" });
            context.Settings.Add(new Setting { Name = "Home Title", Value = "Home Title" });
            context.Settings.Add(new Setting { Name = "Blog Name", Value = "Blog Name" });
            context.Settings.Add(new Setting { Name = "Blog Url", Value = "Blog Url" });
            context.Settings.Add(new Setting { Name = "Author", Value = "Author" });
            context.Settings.Add(new Setting { Name = "GitHub Profile", Value = "GitHub Profile" });
            context.Settings.Add(new Setting { Name = "GitHub Badge HTML Code", Value = "GitHub Badge HTML Code" });
            context.Settings.Add(new Setting { Name = "StackOverflow Badge HTML Code", Value = "StackOverflow Badge HTML Code" });
            context.Settings.Add(new Setting { Name = "Stack Overflow Profile", Value = "Stack Overflow Profile" });
            context.Settings.Add(new Setting { Name = "Linked In Profile", Value = "Linked In Profile" });
            context.Settings.Add(new Setting { Name = "Contact Email", Value = "Contact Email" });
            context.Settings.Add(new Setting { Name = "Facebook Profile", Value = "Facebook Profile" });
            context.Settings.Add(new Setting { Name = "Foursquare Profile", Value = "Foursquare Profile" });
            context.Settings.Add(new Setting { Name = "Google Profile", Value = "Google+ Profile" });
            context.Settings.Add(new Setting { Name = "RSS Url", Value = "RSS Url" });
        }

        protected void SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.AdministratorRoleName));
        }
    }
}
