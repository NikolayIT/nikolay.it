namespace BlogSystem.Web.ViewModels.Videos
{
    using System.Collections.Generic;

    public class VideoListViewModel
    {
        public int Page { get; set; }

        public int Pages { get; set; }

        public IEnumerable<VideoListItemViewModel> Videos { get; set; }
    }
}