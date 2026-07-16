namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Data;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration;
    using BlogSystem.Web.ViewModels.Administration.Videos;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class VideosController : AdministrationController
    {
        private const int ItemsPerPage = 20;

        private readonly ILatestVideosProvider latestVideosProvider;

        private readonly IDeletableEntityRepository<Video> videosRepository;

        public VideosController(
            ILatestVideosProvider latestVideosProvider,
            IDeletableEntityRepository<Video> videosRepository)
        {
            this.latestVideosProvider = latestVideosProvider;
            this.videosRepository = videosRepository;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var query = this.videosRepository.AllWithDeleted();
            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / ItemsPerPage));
            page = Math.Clamp(page, 1, totalPages);

            var viewModel = new PaginatedListViewModel<VideoRowViewModel>
            {
                Items = await query
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip((page - 1) * ItemsPerPage)
                    .Take(ItemsPerPage)
                    .To<VideoRowViewModel>()
                    .ToListAsync(),
                PageNumber = page,
                PageSize = ItemsPerPage,
                TotalCount = totalCount,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FetchLatest()
        {
            await this.latestVideosProvider.FetchLatestVideosAsync("UCPlL0QmMNVtubUKO2SqU4zQ", true);
            await this.latestVideosProvider.FetchLatestVideosAsync("UCH9CezHU-ICXwIiL9Q--l4w", false);

            this.TempData["StatusMessage"] = "The latest YouTube videos were fetched.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, string returnUrl = null)
        {
            var video = await this.videosRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            if (video == null)
            {
                return this.NotFound();
            }

            this.videosRepository.Delete(video);
            await this.videosRepository.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Video \"{video.Title}\" was deleted. You can restore it at any time.";
            if (!string.IsNullOrWhiteSpace(returnUrl) && this.Url.IsLocalUrl(returnUrl))
            {
                return this.LocalRedirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.Index), new { page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id, int page = 1)
        {
            var video = await this.videosRepository.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (video == null)
            {
                return this.NotFound();
            }

            this.videosRepository.Undelete(video);
            await this.videosRepository.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Video \"{video.Title}\" was restored.";
            return this.RedirectToAction(nameof(this.Index), new { page });
        }
    }
}
