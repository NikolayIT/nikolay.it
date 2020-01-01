namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels;
    using BlogSystem.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private const int PostsPerPageDefaultValue = 5;
        private readonly IDeletableEntityRepository<BlogPost> blogPostsRepository;

        public HomeController(IDeletableEntityRepository<BlogPost> blogPostsRepository)
        {
            this.blogPostsRepository = blogPostsRepository;
        }

        public IActionResult Index(int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            var pagesCount = (int)Math.Ceiling(this.blogPostsRepository.All().Count() / (decimal)perPage);

            var posts = this.blogPostsRepository
                .All()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
                .To<BlogPostAnnotationViewModel>()
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

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpGet("robots.txt")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public IActionResult RobotsTxt() =>
            this.Content("User-agent: *" + Environment.NewLine + "Disallow:");
    }
}
