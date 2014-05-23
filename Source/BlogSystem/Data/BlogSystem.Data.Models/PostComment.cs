namespace BlogSystem.Data.Models
{
    using BlogSystem.Data.Contracts;

    public class PostComment : DeletableEntity
    {
        public PostComment()
        {
            this.IsVisible = true;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public int BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsVisible { get; set; }
    }
}
