namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data;
    using BlogSystem.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class FeedsController : AdministrationController
    {
        private readonly ApplicationDbContext db;

        public FeedsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: Administration/Feeds
        public async Task<IActionResult> Index()
        {
            return this.View(await this.db.Feeds.ToListAsync());
        }

        // GET: Administration/Feeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var feed = await this.db.Feeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feed == null)
            {
                return this.NotFound();
            }

            return this.View(feed);
        }

        // GET: Administration/Feeds/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Feeds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feed feed)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Add(feed);
                await this.db.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(feed);
        }

        // GET: Administration/Feeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var feed = await this.db.Feeds.FindAsync(id);
            if (feed == null)
            {
                return this.NotFound();
            }

            return this.View(feed);
        }

        // POST: Administration/Feeds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Feed feed)
        {
            if (id != feed.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.db.Update(feed);
                    await this.db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.FeedExists(feed.Id))
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

            return this.View(feed);
        }

        // GET: Administration/Feeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var feed = await this.db.Feeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feed == null)
            {
                return this.NotFound();
            }

            return this.View(feed);
        }

        // POST: Administration/Feeds/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feed = await this.db.Feeds.FindAsync(id);
            this.db.Feeds.Remove(feed);
            await this.db.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool FeedExists(int id)
        {
            return this.db.Feeds.Any(e => e.Id == id);
        }
    }
}
