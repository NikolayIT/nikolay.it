namespace BlogSystem.Web.ViewModels.Administration.BlogPosts
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class BlogPostRowViewModel : IMapFrom<BlogPost>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public BlogPostType Type { get; set; }

        public bool IsPinned { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
