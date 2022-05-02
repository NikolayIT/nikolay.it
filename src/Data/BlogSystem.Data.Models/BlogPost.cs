namespace BlogSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Common.Models;

    public class BlogPost : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        [DataType(DataType.Html)]
        public string Content { get; set; }

        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        [DataType(DataType.Html)]
        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public BlogPostType Type { get; set; }

        public bool IsPinned { get; set; }
    }
}
