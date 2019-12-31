namespace BlogSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Common.Models;

    public class BlogPost : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Content { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public BlogPostType Type { get; set; }

        public int? OldId { get; set; }
    }
}
