namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.ViewModels.Pages;

    public class PagesController : BaseController
    {
        public PagesController(ApplicationDbContext data)
            : base(data)
        {
        }

        public ActionResult Page(string permalink)
        {
            var viewModel =
                this.Data.Pages.Where(x => x.Permalink.ToLower().Trim() == permalink.ToLower().Trim())
                    .Select(
                        x =>
                        new PageViewModel
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Content = x.Content,
                                LastModified = x.ModifiedOn ?? x.CreatedOn,
                            })
                    .FirstOrDefault();

            if (viewModel == null)
            {
                return this.HttpNotFound("Page not found!");
            }

            return this.View(viewModel);
        }
    }
}
