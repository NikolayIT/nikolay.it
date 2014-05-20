namespace BlogSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BlogPost : ContentHolder
    {
        public BlogPost()
        {
            this.Comments = new HashSet<PostComment>();
        }

        public int? OldId { get; set; }

        [DataType(DataType.Html)]
        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public BlogPostType Type { get; set; }

        public virtual ICollection<PostComment> Comments { get; set; }
    }
}
