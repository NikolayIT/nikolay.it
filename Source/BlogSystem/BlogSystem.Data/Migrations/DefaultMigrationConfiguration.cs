namespace BlogSystem.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using BlogSystem.Common;

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
            if (context.Roles.Any())
            {
                return;
            }

            this.SeedRoles(context);
        }

        protected void SeedRoles(ApplicationDbContext context)
        {
            foreach (var entity in context.Roles)
            {
                context.Roles.Remove(entity);
            }

            context.Roles.AddOrUpdate(new IdentityRole(GlobalConstants.AdministratorRoleName));
        }
    }
}
