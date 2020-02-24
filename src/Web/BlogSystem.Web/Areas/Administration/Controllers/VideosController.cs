namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BlogSystem.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    public class VideosController : AdministrationController
    {
        private readonly ILatestVideosProvider latestVideosProvider;

        public VideosController(ILatestVideosProvider latestVideosProvider)
        {
            this.latestVideosProvider = latestVideosProvider;
        }

        public async Task<IActionResult> FetchLatest()
        {
            await this.latestVideosProvider.FetchLatestVideosAsync("UCPlL0QmMNVtubUKO2SqU4zQ");
            return this.Redirect("/Videos");
        }
    }
}
