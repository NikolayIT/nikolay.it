namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration.Pages;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class PagesController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Page> pages;

        public PagesController(IDeletableEntityRepository<Page> pages)
        {
            this.pages = pages;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.pages.AllWithDeleted()
                .OrderBy(x => x.Title)
                .To<PageRowViewModel>()
                .ToListAsync();
            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View(new PageInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var page = new Page();
            CopyInputToEntity(input, page);

            await this.pages.AddAsync(page);
            await this.pages.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Page \"{page.Title}\" was created.";
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.pages.AllWithDeleted()
                .Where(x => x.Id == id)
                .To<PageEditViewModel>()
                .FirstOrDefaultAsync();
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PageEditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            var page = await this.pages.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (page == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                input.IsDeleted = page.IsDeleted;
                input.CreatedOn = page.CreatedOn;
                input.ModifiedOn = page.ModifiedOn;
                return this.View(input);
            }

            CopyInputToEntity(input, page);
            await this.pages.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Page \"{page.Title}\" was saved.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var page = await this.pages.All().FirstOrDefaultAsync(x => x.Id == id);
            if (page == null)
            {
                return this.NotFound();
            }

            this.pages.Delete(page);
            await this.pages.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Page \"{page.Title}\" was deleted. You can restore it at any time.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var page = await this.pages.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (page == null)
            {
                return this.NotFound();
            }

            this.pages.Undelete(page);
            await this.pages.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Page \"{page.Title}\" was restored.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            var page = await this.pages.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (page == null)
            {
                return this.NotFound();
            }

            if (!page.IsDeleted)
            {
                this.TempData["ErrorMessage"] = "Only deleted pages can be deleted permanently.";
                return this.RedirectToAction(nameof(this.Index));
            }

            this.pages.HardDelete(page);
            await this.pages.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Page \"{page.Title}\" was permanently deleted.";
            return this.RedirectToAction(nameof(this.Index));
        }

        private static void CopyInputToEntity(PageInputModel input, Page page)
        {
            page.Title = input.Title;
            page.Permalink = input.Permalink;
            page.Content = input.Content;
            page.MetaDescription = input.MetaDescription;
            page.MetaKeywords = input.MetaKeywords;
        }
    }
}
