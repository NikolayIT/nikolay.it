namespace BlogSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using BlogSystem.Data.Models;

    public interface IApplicationDbContext : IDisposable
    {
        IDbSet<BlogPost> BlogPosts { get; set; }

        IDbSet<Page> Pages { get; set; }

        IDbSet<Tag> Tags { get; set; }

        IDbSet<Setting> Settings { get; set; }

        IDbSet<PostComment> Comments { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}
