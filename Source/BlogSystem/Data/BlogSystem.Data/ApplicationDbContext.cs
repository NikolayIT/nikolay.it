namespace BlogSystem.Data
{
    using System.Data.Entity;

    using BlogSystem.Data.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
