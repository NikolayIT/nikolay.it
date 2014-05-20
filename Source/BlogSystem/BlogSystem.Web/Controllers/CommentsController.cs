namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.ViewModels.Comments;

    public class CommentsController : BaseController
    {
        public CommentsController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult All(int id, int maxComments = 999, int startFrom = 0)
        {
            var comments = this.Data.Comments
                .Where(c => !c.IsDeleted && c.IsApproved)
                .OrderByDescending(c => c.CreatedOn)
                .Skip(startFrom)
                .Take(maxComments)
                .Select(c =>
                    new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        BlogPostId = c.BlogPostId,
                        CommentedOn = c.CreatedOn
                    });

            return PartialView(comments);
        }
	}
}