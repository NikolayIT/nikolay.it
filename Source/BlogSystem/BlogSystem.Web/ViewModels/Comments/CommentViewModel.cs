namespace BlogSystem.Web.ViewModels.Comments
{
    using System;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int BlogPostId { get; set; }

        public DateTime CommentedOn { get; set; }
    }
}