namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Filters;
    using BlogSystem.Web.ViewModels.Blog;

    public class BlogController : BaseController
    {
        private readonly IRepository<BlogPost> blogPosts;

        public BlogController(IRepository<BlogPost> blogPosts)
        {
            this.blogPosts = blogPosts;
        }
        
        [PassRouteValuesToViewData]
        public ActionResult Post(int id)
        {
            var viewModel =
                this.blogPosts.All().Where(x => x.Id == id).Project().To<BlogPostViewModel>().FirstOrDefault();

            if (viewModel == null)
            {
                return this.HttpNotFound("Blog post not found");
            }

            this.ViewBag.Keywords = viewModel.MetaKeywords;
            this.ViewBag.Description = viewModel.MetaDescription;

            return this.View(viewModel);
        }
    }
}