namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration.BlogPosts;
    using BlogSystem.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class DashboardController : AdministrationController
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPosts;
        private readonly IDeletableEntityRepository<Page> pages;
        private readonly IDeletableEntityRepository<Video> videos;
        private readonly IDeletableEntityRepository<Setting> settings;

        public DashboardController(
            IDeletableEntityRepository<BlogPost> blogPosts,
            IDeletableEntityRepository<Page> pages,
            IDeletableEntityRepository<Video> videos,
            IDeletableEntityRepository<Setting> settings)
        {
            this.blogPosts = blogPosts;
            this.pages = pages;
            this.videos = videos;
            this.settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                BlogPostsCount = await this.blogPosts.All().CountAsync(),
                PagesCount = await this.pages.All().CountAsync(),
                VideosCount = await this.videos.All().CountAsync(),
                SettingsCount = await this.settings.All().CountAsync(),
                RecentBlogPosts = await this.blogPosts.AllWithDeleted()
                    .OrderByDescending(x => x.CreatedOn)
                    .Take(5)
                    .To<BlogPostRowViewModel>()
                    .ToListAsync(),
            };

            return this.View(viewModel);
        }
    }
}
