namespace BlogSystem.Web.Controllers
{
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Pages;

    using Microsoft.AspNetCore.Mvc;

    public class PagesController : BaseController
    {
        private readonly IDeletableEntityRepository<Page> pagesRepository;

        public PagesController(IDeletableEntityRepository<Page> pages)
        {
            this.pagesRepository = pages;
        }

        public ActionResult Page(string permalink)
        {
            var viewModel =
                this.pagesRepository.All()
                    .Where(x => x.Permalink.Trim() == permalink.Trim())
                    .To<PageViewModel>()
                    .FirstOrDefault();

            if (viewModel == null)
            {
                return this.NotFound("Page not found!");
            }

            return this.View(viewModel);
        }
    }
}
