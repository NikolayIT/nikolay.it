namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels.Comments;
    using BlogSystem.Data.Contracts;
    using BlogSystem.Web.Infrastructure.Filters;

    public class CommentsController : BaseController
    {
        private IRepository<PostComment> commentsData;

        public CommentsController(IRepository<PostComment> commentsRepository)
        {
            this.commentsData = commentsRepository;
        }

        [PassRouteValuesToViewData]
        public ActionResult All(int id, int maxComments = 999, int startFrom = 0)
        {
            var comments = commentsData
                .All()
                .Where(c => !c.IsDeleted && c.IsVisible)
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

        public ActionResult Create(int id, CommentViewModel comment)
        {
            var newComment = new PostComment
            {
                Content = comment.Content,
                BlogPostId = id,
                CreatedOn = DateTime.Now,
            };

            throw new NotImplementedException();
        }
	}
}