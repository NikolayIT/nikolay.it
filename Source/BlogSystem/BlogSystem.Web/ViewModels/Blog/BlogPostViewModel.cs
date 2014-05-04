namespace BlogSystem.Web.ViewModels.Blog
{
    using System;

    public class BlogPostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}