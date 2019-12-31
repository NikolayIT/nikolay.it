namespace BlogSystem.Web.ViewModels.Videos
{
    using System.Collections.Generic;

    public class VideoListViewModel
    {
        public int Page { get; set; }

        public int Pages { get; set; }

        public IEnumerable<VideoListItemViewModel> Videos { get; set; }

        public int NextPage
        {
            get
            {
                if (this.Page >= this.Pages)
                {
                    return 1;
                }

                return this.Page + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.Page <= 1)
                {
                    return this.Pages;
                }

                return this.Page - 1;
            }
        }
    }
}
