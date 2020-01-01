namespace BlogSystem.Web.Controllers
{
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services;

    using Microsoft.AspNetCore.Mvc;

    public class RedirectsController : BaseController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;

        public RedirectsController(IDeletableEntityRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        public ActionResult OldSystemBlogPost(int id)
        {
            var blogPost = this.blogPosts.All().Select(x => new { x.Id, x.Title, x.CreatedOn, x.OldId })
                .FirstOrDefault(x => x.OldId == id);
            if (blogPost == null)
            {
                return this.NotFound("Blog post not found");
            }

            IBlogUrlGenerator blogUrlGenerator = new BlogUrlGenerator();
            var url = blogUrlGenerator.GenerateUrl(blogPost.Id, blogPost.Title, blogPost.CreatedOn);

            return this.RedirectPermanent(url);
        }

        public ActionResult Index()
        {
            return this.RedirectPermanent("/");
        }
    }
}
