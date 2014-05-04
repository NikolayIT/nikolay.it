namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.Helpers;

    public class RedirectsController : BaseController
    {
        public RedirectsController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult OldSystemBlogPost(int id)
        {
            var blogPost = this.Data.BlogPosts.Select(x => new { x.Id, x.Title, x.CreatedOn, x.OldId }).FirstOrDefault(x => x.OldId == id);
            if (blogPost == null)
            {
                return this.HttpNotFound("Blog post not found");
            }

            IBlogUrlGenerator blogUrlGenerator = new BlogUrlGenerator();
            var url = blogUrlGenerator.GenerateUrl(blogPost.Id, blogPost.Title, blogPost.CreatedOn);

            return this.RedirectPermanent(url);
        }
    }
}