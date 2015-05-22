namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Helpers;

    public class RedirectsController : BaseController
    {
        private readonly IRepository<BlogPost> blogPosts;

        public RedirectsController(IRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        public ActionResult OldSystemBlogPost(int id)
        {
            var blogPost = this.blogPosts.All().Select(x => new { x.Id, x.Title, x.CreatedOn, x.OldId }).FirstOrDefault(x => x.OldId == id);
            if (blogPost == null)
            {
                return this.HttpNotFound("Blog post not found");
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