namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Videos;

    using Microsoft.AspNetCore.Mvc;

    public class VideosController : BaseController
    {
        private const int ItemsPerPage = 12;
        private readonly IDeletableEntityRepository<Video> videosRepository;

        public VideosController(IDeletableEntityRepository<Video> videos)
        {
            this.videosRepository = videos;
        }

        public ActionResult Index(int page = 1)
        {
            var pagesCount = (int)Math.Ceiling(this.videosRepository.All().Count() / (decimal)ItemsPerPage);

            var videos = this.videosRepository
                .All()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
                .To<VideoListItemViewModel>()
                .Skip(ItemsPerPage * (page - 1))
                .Take(ItemsPerPage)
                .ToList();

            var model = new VideoListViewModel { Page = page, Pages = pagesCount, Videos = videos };
            return this.View(model);
        }
    }
}
