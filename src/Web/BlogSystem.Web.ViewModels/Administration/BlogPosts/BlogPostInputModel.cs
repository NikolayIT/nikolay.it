namespace BlogSystem.Web.ViewModels.Administration.BlogPosts
{
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Data.Models;

    public class BlogPostInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "Type")]
        public BlogPostType Type { get; set; }

        [Display(Name = "Image or video URL")]
        [MaxLength(2000)]
        public string ImageOrVideoUrl { get; set; }

        [Display(Name = "Short content")]
        [DataType(DataType.Html)]
        public string ShortContent { get; set; }

        [DataType(DataType.Html)]
        public string Content { get; set; }

        [Display(Name = "Meta description")]
        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        [Display(Name = "Meta keywords")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Pin this post to the top")]
        public bool IsPinned { get; set; }
    }
}
