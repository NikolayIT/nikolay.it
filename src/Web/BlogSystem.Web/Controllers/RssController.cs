namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;

    using BlogSystem.Common;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services;

    using Microsoft.AspNetCore.Mvc;

    public class RssController : BaseController
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

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.AppendLine("<rss version=\"2.0\">");
            sb.AppendLine("<channel>");
            sb.AppendLine($"<title>{GlobalConstants.SystemName}</title>");
            sb.AppendLine($"<link>{GlobalConstants.SystemBaseUrl}</link>");
            sb.AppendLine($"<description>{GlobalConstants.SystemName}</description>");

            foreach (var post in posts)
            {
                var url = this.urlGenerator.GenerateUrl(post.Id, post.Title, post.CreatedOn);
                sb.AppendLine(
$@"<item>
    <title>{WebUtility.HtmlEncode(post.Title)}</title>
    <link>{GlobalConstants.SystemBaseUrl}{url}</link>
    <description>{WebUtility.HtmlEncode(post.ShortContent)}</description>
    <pubDate>{post.CreatedOn:ddd, dd MMM yyyy HH:mm:ss zzz}</pubDate>
    <guid>{GlobalConstants.SystemBaseUrl}{url}</guid>
</item>");
            }

            sb.AppendLine("</channel>");
            sb.AppendLine("</rss>");
            return this.Content(sb.ToString(), "application/xml");
        }
    }
}
