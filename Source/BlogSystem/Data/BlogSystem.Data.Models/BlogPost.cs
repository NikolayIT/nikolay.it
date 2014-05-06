namespace BlogSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BlogPost : ContentHolder
    {
        public int? OldId { get; set; }

        [DataType(DataType.Html)]
        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public BlogPostType Type { get; set; }
    }
}
