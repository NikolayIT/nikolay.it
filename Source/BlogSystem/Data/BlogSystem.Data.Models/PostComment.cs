namespace BlogSystem.Data.Models
{
    using BlogSystem.Data.Contracts;
    using System.ComponentModel;

    public class PostComment : DeletableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual ApplicationUser User { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }
    }
}
