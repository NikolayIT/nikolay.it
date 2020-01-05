namespace BlogSystem.Web.ViewComponents
{
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels;
    using BlogSystem.Web.ViewModels.Videos;

    using Microsoft.AspNetCore.Mvc;

    public class SidebarViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPostsRepository;

        private readonly IDeletableEntityRepository<Video> videosRepository;

        public SidebarViewComponent(
            IDeletableEntityRepository<BlogPost> blogPostsRepository,
            IDeletableEntityRepository<Video> videosRepository)
        {
            this.blogPostsRepository = blogPostsRepository;
            this.videosRepository = videosRepository;
        }

        public IViewComponentResult Invoke()
        {
            var model = new SidebarViewModel
                            {
                                RecentPosts =
                                    this.blogPostsRepository.All().OrderByDescending(x => x.CreatedOn)
                                        .To<RecentBlogPostViewModel>().Take(5).ToList(),
                                RecentVideos = this.videosRepository.All().OrderByDescending(x => x.CreatedOn).Take(5)
                                    .To<VideoListItemViewModel>().ToList(),
                            };

            return this.View(model);
        }
    }
}
