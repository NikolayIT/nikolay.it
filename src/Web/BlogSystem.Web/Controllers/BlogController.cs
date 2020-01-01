namespace BlogSystem.Web.Controllers
{
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Blog;

    using Microsoft.AspNetCore.Mvc;

    public class BlogController : BaseController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;

        public BlogController(IDeletableEntityRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }

        public ActionResult Post(int id)
        {
            var viewModel =
                this.blogPosts.All().Where(x => x.Id == id).To<BlogPostViewModel>().FirstOrDefault();

            if (viewModel == null)
            {
                return this.NotFound("Blog post not found");
            }

            this.ViewBag.Keywords = viewModel.MetaKeywords;
            this.ViewBag.Description = viewModel.MetaDescription;

            return this.View(viewModel);
        }
    }
}
