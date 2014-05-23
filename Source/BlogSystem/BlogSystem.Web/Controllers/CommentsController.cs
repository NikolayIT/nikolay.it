namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;
    using BlogSystem.Data.Models;
    using BlogSystem.Web.ViewModels.Comments;
    using BlogSystem.Data.Contracts;
    using BlogSystem.Web.Infrastructure.Filters;
    using BlogSystem.Web.Infrastructure.Identity;

    public class CommentsController : BaseController
    {
        private const int MinutesBetweenComments = 1;

        private readonly IRepository<PostComment> commentsData;
        private readonly ICurrentUser currentUser;

        public CommentsController(IRepository<PostComment> commentsRepository, ICurrentUser user)
        {
            commentsData = commentsRepository;
            currentUser = user;
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
                .Project()
                .To<CommentViewModel>();

            return PartialView(comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, CommentViewModel comment)
        {
            if (CurrentUserCommentedInLastMinutes())
            {
                return JsonError(string.Format("You can comment every {0} minute", MinutesBetweenComments));
            }

            if (ModelState.IsValid)
            {
                var newComment = new PostComment
                {
                    Content = comment.Content,
                    BlogPostId = id,
                    User = currentUser.Get()
                };

                commentsData.Add(newComment);
                commentsData.SaveChanges();

                comment.User = currentUser.Get().UserName;
                comment.CommentedOn = DateTime.Now;

                return PartialView("_CommentDetail", comment);
            }
            else
            {
                return JsonError("Content is required");
            }
        }

        private bool CurrentUserCommentedInLastMinutes()
        {
            var lastCommentDate = currentUser.Get()
                .Comments
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefault()
                .CreatedOn;

            return lastCommentDate.AddMinutes(MinutesBetweenComments) >= DateTime.Now;
        }
    }
}