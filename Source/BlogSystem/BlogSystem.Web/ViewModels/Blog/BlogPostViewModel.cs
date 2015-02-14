namespace BlogSystem.Web.ViewModels.Blog
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class BlogPostViewModel : IMapFrom<BlogPost>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Content { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}