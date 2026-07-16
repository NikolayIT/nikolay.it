namespace BlogSystem.Web.ViewModels.Administration.BlogPosts
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class BlogPostEditViewModel : BlogPostInputModel, IMapFrom<BlogPost>
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
