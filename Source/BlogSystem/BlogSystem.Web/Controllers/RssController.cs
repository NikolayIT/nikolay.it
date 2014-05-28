namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Helpers;
    using BlogSystem.Web.Infrastructure.ActionResults;

    public class RssController : Controller
    {
        private readonly IRepository<BlogPost> blogPostsData;
        private readonly IBlogUrlGenerator urlGenerator;

        public RssController(IRepository<BlogPost> blogPosts, IBlogUrlGenerator urlGenerator)
        {
            this.blogPostsData = blogPosts;
            this.urlGenerator = urlGenerator;
        }

        public ActionResult Blog()
        {
            var posts =
                this.blogPostsData.All()
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new { x.Id, x.Title, x.ShortContent, x.CreatedOn })
                    .Take(10)
                    .ToList();

            var items = new List<SyndicationItem>();
            foreach (var post in posts)
            {
                var url = this.urlGenerator.GenerateUrl(post.Id, post.Title, post.CreatedOn);
                var item = new SyndicationItem(post.Title, post.ShortContent, new Uri("http://nikolay.it" + url))
                               {
                                   PublishDate = new DateTimeOffset(post.CreatedOn),
                                   Id = post.Id.ToString(CultureInfo.InvariantCulture)
                               };
                items.Add(item);
            }

            var feed = new SyndicationFeed("Nikolay.IT Blog", "Nikolay.IT Blog", new Uri("http://nikolay.it"), items);
            feed.LastUpdatedTime = new DateTimeOffset(DateTime.Now);
            
            return new RssActionResult(feed);
        }
    }
}