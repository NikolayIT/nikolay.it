namespace BlogSystem.Tools.MigrateOldWebsite
{
    using System.Data.Entity;

    using BlogSystem.Data;
    using BlogSystem.Data.Migrations;
    using BlogSystem.Data.Models;

    internal class Program
    {
        private static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, DefaultMigrationConfiguration>());

            var oldDatabase = new nikolayitEntities();
            var newDatabase = new ApplicationDbContext();

            foreach (var post in oldDatabase.posts)
            {
                var newPost = new BlogPost
                                  {
                                      Content = post.content,
                                      CreatedOn = post.published,
                                      PreserveCreatedOn = true,
                                      MetaDescription = post.meta_description,
                                      MetaKeywords = post.meta_keywords,
                                      ShortContent = post.content_short,
                                      ModifiedOn = post.last_update,
                                      SubTitle = post.subtitle,
                                      Title = post.title,
                                  };

                newDatabase.BlogPosts.Add(newPost);
            }

            newDatabase.SaveChanges();
        }
    }
}
