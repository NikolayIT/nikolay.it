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
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var data = new ApplicationDbContext();

            this.ViewBag.RecentPost =
                data.BlogPosts.OrderByDescending(x => x.CreatedOn)
                    .Select(x => new RecentBlogPostViewModel { Title = x.Title, Id = x.Id, CreatedOn = x.CreatedOn, Type = x.Type })
                    .Take(5);

            this.ViewBag.Tags = data.Tags.OrderBy(x => x.Name).Select(x => new TagViewModel { Name = x.Name, PostsCount = x.BlogPosts.Count });

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}