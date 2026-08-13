namespace BlogSystem.Web.Controllers
{
    using System.Linq;

    using BlogSystem.Common;
    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Blog;

    using Microsoft.AspNetCore.Mvc;

    public class BlogController : BaseController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;
        private readonly IBlogUrlGenerator urlGenerator;
        private readonly IYouTubeUrlParser youTubeUrlParser;

        public BlogController(
            IDeletableEntityRepository<BlogPost> blogPosts,
            IBlogUrlGenerator urlGenerator,
            IYouTubeUrlParser youTubeUrlParser)
        {
            this.blogPosts = blogPosts;
            this.urlGenerator = urlGenerator;
            this.youTubeUrlParser = youTubeUrlParser;
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
            this.ViewBag.Canonical = GlobalConstants.SystemBaseUrl +
                this.urlGenerator.GenerateUrl(viewModel.Id, viewModel.Title, viewModel.CreatedOn);
            this.ViewBag.OgType = "article";
            this.ViewBag.OgImage =
                this.youTubeUrlParser.ToAbsoluteImageUrl(viewModel.ImageOrVideoUrl, GlobalConstants.SystemBaseUrl);

            return this.View(viewModel);
        }
    }
}
