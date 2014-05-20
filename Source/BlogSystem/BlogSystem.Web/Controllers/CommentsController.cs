namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels.Comments;
    using BlogSystem.Data.Contracts;

    public class CommentsController : BaseController
    {
        private IRepository<PostComment> commentsData;

        public CommentsController(IRepository<PostComment> commentsRepository)
        {
            this.commentsData = commentsRepository;
        }

        public ActionResult All(int id, int maxComments = 999, int startFrom = 0)
        {
            var comments = this.commentsData
                .All()
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