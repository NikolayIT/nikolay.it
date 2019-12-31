namespace BlogSystem.Web.ViewModels
{
    using System.Collections.Generic;

    using BlogSystem.Web.ViewModels.Videos;

    public class SidebarViewModel
    {
        public IEnumerable<RecentBlogPostViewModel> RecentPosts { get; set; }

        public IEnumerable<VideoListItemViewModel> RecentVideos { get; set; }
    }
}
