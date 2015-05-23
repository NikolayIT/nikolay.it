namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels.Videos;

    public class VideosController : BaseController
    {
        private const int ItemsPerPage = 20;
        private readonly IRepository<Video> videosRepository;

        public VideosController(IRepository<Video> videos)
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
                .Project().To<VideoListItemViewModel>()
                .Skip(ItemsPerPage * (page - 1))
                .Take(ItemsPerPage)
                .ToList();

            var model = new VideoListViewModel { Page = page, Pages = pagesCount, Videos = videos };
            return this.View(model);
        }
    }
}
