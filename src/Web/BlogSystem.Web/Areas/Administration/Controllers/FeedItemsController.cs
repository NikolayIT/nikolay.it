namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Administration.FeedItems;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class FeedItemsController : AdministrationController
    {
        private readonly ApplicationDbContext dbContext;

        public FeedItemsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var query = this.dbContext.FeedItems.AsQueryable();
            query = id.HasValue ? query.Where(x => x.FeedId == id.Value) : query.Where(x => !x.IsRead);
            var items = await query.OrderByDescending(x => x.CreatedOn).To<FeedItemViewModel>().ToListAsync();
            return this.View(items);
        }

        public async Task<IActionResult> MarkAsRead(int id)
        {
            var item = await this.dbContext.FeedItems.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                item.IsRead = true;
                await this.dbContext.SaveChangesAsync();
            }

            return this.Ok();
        }

        public async Task<IActionResult> MarkAsUnread(int id)
        {
            var item = await this.dbContext.FeedItems.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                item.IsRead = false;
                await this.dbContext.SaveChangesAsync();
            }

            return this.Ok();
        }
    }
}
