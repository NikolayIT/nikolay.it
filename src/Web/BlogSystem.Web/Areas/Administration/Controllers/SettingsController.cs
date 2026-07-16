namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration.Settings;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class SettingsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Setting> settings;

        public SettingsController(IDeletableEntityRepository<Setting> settings)
        {
            this.settings = settings;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.settings.AllWithDeleted()
                .OrderBy(x => x.Name)
                .To<SettingRowViewModel>()
                .ToListAsync();
            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View(new SettingInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var setting = new Setting { Name = input.Name, Value = input.Value };
            await this.settings.AddAsync(setting);
            await this.settings.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Setting \"{setting.Name}\" was created.";
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.settings.AllWithDeleted()
                .Where(x => x.Id == id)
                .To<SettingEditViewModel>()
                .FirstOrDefaultAsync();
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SettingEditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            var setting = await this.settings.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                input.CreatedOn = setting.CreatedOn;
                input.ModifiedOn = setting.ModifiedOn;
                return this.View(input);
            }

            setting.Name = input.Name;
            setting.Value = input.Value;
            await this.settings.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Setting \"{setting.Name}\" was saved.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var setting = await this.settings.All().FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            this.settings.Delete(setting);
            await this.settings.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Setting \"{setting.Name}\" was deleted. You can restore it at any time.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var setting = await this.settings.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            this.settings.Undelete(setting);
            await this.settings.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Setting \"{setting.Name}\" was restored.";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            var setting = await this.settings.AllWithDeleted().FirstOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            if (!setting.IsDeleted)
            {
                this.TempData["ErrorMessage"] = "Only deleted settings can be deleted permanently.";
                return this.RedirectToAction(nameof(this.Index));
            }

            this.settings.HardDelete(setting);
            await this.settings.SaveChangesAsync();

            this.TempData["StatusMessage"] = $"Setting \"{setting.Name}\" was permanently deleted.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
