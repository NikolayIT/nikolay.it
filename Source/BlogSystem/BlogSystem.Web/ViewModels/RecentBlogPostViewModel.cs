namespace BlogSystem.Web.ViewModels
{
    using System;

    using BlogSystem.Web.Helpers;

    public class RecentBlogPostViewModel
    {
        
        private readonly IBlogUrlGenerator urlGenerator;

        public RecentBlogPostViewModel()
            : this(new BlogUrlGenerator())
        {
        }

        public RecentBlogPostViewModel(IBlogUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Image
        {
            get
            {
                return "http://cainclusion.org/camap/images/resources/video.png";
            }
        }

        public string Url
        {
            get
            {
                return this.urlGenerator.GenerateUrl(this.Id, this.Title, this.CreatedOn);
            }
        }
    }
}