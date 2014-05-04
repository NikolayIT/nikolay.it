namespace BlogSystem.Web.ViewModels.Home
{
    using System;

    using BlogSystem.Web.Helpers;

    public class BlogPostAnnotationViewModel
    {
        private IBlogUrlGenerator urlGenerator;

        public BlogPostAnnotationViewModel()
            : this(new BlogUrlGenerator())
        {
        }

        public BlogPostAnnotationViewModel(IBlogUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public string Url
        {
            get
            {
                return this.urlGenerator.GenerateUrl(this.Id, this.Title, this.CreatedOn);
            }
        }
    }
}