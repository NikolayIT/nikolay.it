namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private const int PostsPerPageDefaultValue = 5;

        public HomeController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult Index(int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            var pagesCount = (int)Math.Ceiling(this.Data.BlogPosts.Count() / (decimal)perPage);

            var posts =
                this.Data.BlogPosts.Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(
                        x =>
                        new BlogPostAnnotationViewModel
                            {
                                Title = x.Title,
                                Content = x.ShortContent,
                                CreatedOn = x.CreatedOn,
                                ImageOrVideoUrl = x.ImageOrVideoUrl,
                            })
                    .Skip(perPage * (page - 1))
                    .Take(perPage);

            var model = new IndexViewModel
                            {
                                Posts = posts.ToList(),
                                CurrentPage = page,
                                PagesCount = pagesCount,
                            };

            return this.View(model);
        }
    }
}