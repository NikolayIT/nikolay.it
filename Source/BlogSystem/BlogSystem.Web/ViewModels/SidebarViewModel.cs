namespace BlogSystem.Web.ViewModels
{
    using System.Linq;

    public class SidebarViewModel
    {
        public IQueryable<RecentBlogPostViewModel> RecentPosts { get; set; }

        public IOrderedQueryable<TagViewModel> Tags { get; set; }
    }
}