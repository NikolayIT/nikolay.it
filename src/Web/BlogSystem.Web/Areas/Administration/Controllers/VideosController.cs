namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    public class VideosController : AdministrationController
    {
        private readonly ILatestVideosProvider latestVideosProvider;

        private readonly IDeletableEntityRepository<Video> videosRepository;

        public VideosController(
            ILatestVideosProvider latestVideosProvider,
            IDeletableEntityRepository<Video> videosRepository)
        {
            this.latestVideosProvider = latestVideosProvider;
            this.videosRepository = videosRepository;
        }

        public async Task<IActionResult> FetchLatest()
        {
            await this.latestVideosProvider.FetchLatestVideosAsync("UCPlL0QmMNVtubUKO2SqU4zQ", true);
            await this.latestVideosProvider.FetchLatestVideosAsync("UCH9CezHU-ICXwIiL9Q--l4w", false);
            return this.Redirect("/Videos");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var video = this.videosRepository.AllWithDeleted().FirstOrDefault(x => x.Id == id);
            this.videosRepository.Delete(video);
            await this.videosRepository.SaveChangesAsync();
            return this.RedirectToAction("Index", "Videos", new { area = string.Empty });
        }
    }
}
