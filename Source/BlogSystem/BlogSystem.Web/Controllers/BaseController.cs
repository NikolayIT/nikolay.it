namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using BlogSystem.Data;
    using BlogSystem.Web.ViewModels;

    public class BaseController : Controller
    {
        public BaseController(ApplicationDbContext data)
        {
            this.Data = data;
        }

        public ApplicationDbContext Data { get; set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.ViewBag.RecentPost =
                this.Data.BlogPosts.OrderByDescending(x => x.CreatedOn)
                    .Select(x => new RecentBlogPostViewModel { Title = x.Title, Id = x.Id, CreatedOn = x.CreatedOn, Type = x.Type })
                    .Take(5);

            this.ViewBag.Tags = this.Data.Tags.OrderBy(x => x.Name).Select(x => new TagViewModel { Name = x.Name, PostsCount = x.BlogPosts.Count });

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}