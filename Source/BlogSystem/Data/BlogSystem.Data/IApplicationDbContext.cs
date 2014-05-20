namespace BlogSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using BlogSystem.Data.Models;

    public interface IApplicationDbContext : IDisposable
    {
        DbSet<BlogPost> BlogPosts { get; }

        DbSet<Page> Pages { get; }

        DbSet<Tag> Tags { get; }

        DbSet<Setting> Settings { get; }

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}
