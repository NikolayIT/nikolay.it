namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;

    using BlogSystem.Common;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services;

    using Microsoft.AspNetCore.Mvc;

    public class SitemapController : BaseController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;
        private readonly IDeletableEntityRepository<Page> pages;
        private readonly IBlogUrlGenerator urlGenerator;

        public SitemapController(
            IDeletableEntityRepository<BlogPost> blogPosts,
            IDeletableEntityRepository<Page> pages,
            IBlogUrlGenerator urlGenerator)
        {
            this.blogPosts = blogPosts;
            this.pages = pages;
            this.urlGenerator = urlGenerator;
        }

        [HttpGet("sitemap.xml")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            var posts = this.blogPosts.All()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new { x.Id, x.Title, x.CreatedOn, x.ModifiedOn })
                .ToList();

            var staticPages = this.pages.All()
                .Select(x => new { x.Permalink, x.CreatedOn, x.ModifiedOn })
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

            AppendUrl(sb, GlobalConstants.SystemBaseUrl + "/", null, "daily", "1.0");
            AppendUrl(sb, GlobalConstants.SystemBaseUrl + "/Videos", null, "weekly", "0.6");

            foreach (var post in posts)
            {
                var url = GlobalConstants.SystemBaseUrl + this.urlGenerator.GenerateUrl(post.Id, post.Title, post.CreatedOn);
                AppendUrl(sb, url, post.ModifiedOn ?? post.CreatedOn, "yearly", "0.8");
            }

            foreach (var page in staticPages)
            {
                var url = $"{GlobalConstants.SystemBaseUrl}/Pages/{page.Permalink}";
                AppendUrl(sb, url, page.ModifiedOn ?? page.CreatedOn, "yearly", "0.5");
            }

            sb.AppendLine("</urlset>");
            return this.Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }

        private static void AppendUrl(StringBuilder sb, string url, DateTime? lastModified, string changeFrequency, string priority)
        {
            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{WebUtility.HtmlEncode(url)}</loc>");
            if (lastModified != null)
            {
                sb.AppendLine($"<lastmod>{lastModified.Value:yyyy-MM-dd}</lastmod>");
            }

            sb.AppendLine($"<changefreq>{changeFrequency}</changefreq>");
            sb.AppendLine($"<priority>{priority}</priority>");
            sb.AppendLine("</url>");
        }
    }
}
