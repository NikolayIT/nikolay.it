namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Area("Administration")]
    public class SettingsController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Setting> repository;

        public SettingsController(IDeletableEntityRepository<Setting> repository)
        {
            this.repository = repository;
        }

        // GET: Administration/Settings
        public async Task<IActionResult> Index()
        {
            return this.View(await this.repository.AllWithDeleted().ToListAsync());
        }

        // GET: Administration/Settings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var setting = await this.repository.AllWithDeleted()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            return this.View(setting);
        }

        // GET: Administration/Settings/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Settings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Value,Id")] Setting setting)
        {
            if (this.ModelState.IsValid)
            {
                await this.repository.AddAsync(setting);
                await this.repository.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(setting);
        }

        // GET: Administration/Settings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var setting = this.repository.All().FirstOrDefault(x => x.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            return this.View(setting);
        }

        // POST: Administration/Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value")] Setting setting)
        {
            if (id != setting.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var dbSetting = this.repository.AllWithDeleted().FirstOrDefault(x => x.Id == id);
                    dbSetting.Name = setting.Name;
                    dbSetting.Value = setting.Value;
                    await this.repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.SettingExists(setting.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(setting);
        }

        // GET: Administration/Settings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var setting = await this.repository.AllWithDeleted()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return this.NotFound();
            }

            return this.View(setting);
        }

        // POST: Administration/Settings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var setting = this.repository.All().FirstOrDefault(x => x.Id == id);
            this.repository.Delete(setting);
            await this.repository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool SettingExists(int id)
        {
            return this.repository.All().Any(e => e.Id == id);
        }
    }
}
