namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.ViewModels.Index;

    public class HomeController : BaseController
    {
        private const int PostsPerPageDefaultValue = 5;

        public HomeController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult Index(int? page, int? perPage)
        {
            var currentPage = page ?? 1;
            var postsPerPage = perPage ?? PostsPerPageDefaultValue;

            var pagesCount = (int)Math.Ceiling(this.Data.BlogPosts.Count() / (decimal)postsPerPage);

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
                    .Skip(postsPerPage * (currentPage - 1))
                    .Take(postsPerPage);

            var model = new IndexViewModel
                            {
                                Posts = posts.ToList(),
                                CurrentPage = currentPage,
                                PagesCount = pagesCount,
                            };

            return this.View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return this.View();
        }
    }
}