namespace BlogSystem.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    using BlogSystem.Web.ViewModels.Administration.BlogPosts;

    public class DashboardViewModel
    {
        public int BlogPostsCount { get; set; }

        public int PagesCount { get; set; }

        public int VideosCount { get; set; }

        public int SettingsCount { get; set; }

        public IEnumerable<BlogPostRowViewModel> RecentBlogPosts { get; set; }
    }
}
