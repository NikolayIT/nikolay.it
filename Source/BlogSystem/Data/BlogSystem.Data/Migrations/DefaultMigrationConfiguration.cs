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

        private void SeedSettings(ApplicationDbContext context)
        {
            if (context.Settings.Any())
            {
                return;
            }

            context.Settings.Add(new Setting { Name = "Logo Url", Value = null });
            context.Settings.Add(new Setting { Name = "Home Title", Value = null });
            context.Settings.Add(new Setting { Name = "Blog Name", Value = null });
            context.Settings.Add(new Setting { Name = "Author", Value = null });
            context.Settings.Add(new Setting { Name = "GitHub Profile", Value = null });
            context.Settings.Add(new Setting { Name = "Stack Overflow Profile", Value = null });
            context.Settings.Add(new Setting { Name = "Linked In Profile", Value = null });
            context.Settings.Add(new Setting { Name = "Contact Email", Value = null });
            context.Settings.Add(new Setting { Name = "Facebook Profile", Value = null });
            context.Settings.Add(new Setting { Name = "Foursquare Profile", Value = null });
            context.Settings.Add(new Setting { Name = "Google+ Profile", Value = null });
            context.Settings.Add(new Setting { Name = "RSS Url", Value = null });
        }

        protected void SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            foreach (var entity in context.Roles)
            {
                context.Roles.Remove(entity);
            }

            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.AdministratorRoleName));
        }
    }
}
