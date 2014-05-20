namespace BlogSystem.Data.Models
{
    using BlogSystem.Data.Contracts;

    public class PostComment : DeletableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public bool IsApproved { get; set; }
    }
}
