namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.ViewModels.Blog;

    public class BlogController : BaseController
    {
        public BlogController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult Post(int id)
        {
            var viewModel =
                this.Data.BlogPosts.Select(
                    x =>
                    new BlogPostViewModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            SubTitle = x.SubTitle,
                            Content = x.Content,
                            CreatedOn = x.CreatedOn
                        }).FirstOrDefault(x => x.Id == id);

            return this.View(viewModel);
        }
    }
}