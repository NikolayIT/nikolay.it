namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using BlogSystem.Data.Contracts;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Filters;
    using BlogSystem.Web.Infrastructure.Identity;
    using BlogSystem.Web.ViewModels.Comments;

    public class CommentsController : BaseController
    {
        private const int MinutesBetweenComments = 1;

        private readonly IRepository<PostComment> commentsData;
        private readonly ICurrentUser currentUser;

        public CommentsController(IRepository<PostComment> commentsRepository, ICurrentUser user)
        {
            this.commentsData = commentsRepository;
            this.currentUser = user;
        }

        [PassRouteValuesToViewData]
        public ActionResult All(int id, int maxComments = 999, int startFrom = 0)
        {
            var comments = this.commentsData
                .All()
                .Where(c => !c.IsDeleted && c.IsVisible)
                .OrderByDescending(c => c.CreatedOn)
                .Skip(startFrom)
                .Take(maxComments)
                .Project()
                .To<CommentViewModel>();

            return this.PartialView(comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, CommentViewModel comment)
        {
            if (this.CurrentUserCommentedInLastMinutes())
            {
                return this.JsonError(string.Format("You can comment every {0} minute", MinutesBetweenComments));
            }

            if (ModelState.IsValid)
            {
                var newComment = new PostComment
                {
                    Content = comment.Content,
                    BlogPostId = id,
                    User = this.currentUser.Get()
                };

                this.commentsData.Add(newComment);
                this.commentsData.SaveChanges();

                comment.User = this.currentUser.Get().UserName;
                comment.CommentedOn = DateTime.Now;

                return this.PartialView("_CommentDetail", comment);
            }
            else
            {
                return this.JsonError("Content is required");
            }
        }

        private bool CurrentUserCommentedInLastMinutes()
        {
            var lastComment = this.currentUser.Get()
                .Comments
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefault();

            if (lastComment == null)
            {
                return false;
            }

            return lastComment.CreatedOn.AddMinutes(MinutesBetweenComments) >= DateTime.Now;
        }
    }
}