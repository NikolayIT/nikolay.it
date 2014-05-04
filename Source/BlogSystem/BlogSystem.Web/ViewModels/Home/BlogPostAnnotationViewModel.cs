namespace BlogSystem.Web.ViewModels.Home
{
    using System;

    public class BlogPostAnnotationViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageOrVideoUrl { get; set; }
    }
}