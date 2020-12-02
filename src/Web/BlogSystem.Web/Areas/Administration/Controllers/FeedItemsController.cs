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

        public async Task<IActionResult> Index()
        {
            var items = await this.dbContext.FeedItems.Where(x => !x.IsRead).To<FeedItemViewModel>().ToListAsync();
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
    }
}
