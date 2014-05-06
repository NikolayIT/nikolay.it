namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels.Blog;

    public class BlogController : BaseController
    {
        private readonly IRepository<BlogPost> blogPosts;

        public BlogController(IRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        public ActionResult Post(int id)
        {
            var viewModel =
                this.blogPosts.All().Select(
                    x =>
                    new BlogPostViewModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            SubTitle = x.SubTitle,
                            Content = x.Content,
                            CreatedOn = x.CreatedOn
                        }).FirstOrDefault(x => x.Id == id);

            if (viewModel == null)
            {
                return this.HttpNotFound("Blog post not found");
            }

            return this.View(viewModel);
        }
    }
}