namespace BlogSystem.Web.ViewComponents
{
    using System.Linq;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;
    using BlogSystem.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class MenuViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<Page> pagesRepository;

        public MenuViewComponent(IDeletableEntityRepository<Page> pagesRepository)
        {
            this.pagesRepository = pagesRepository;
        }

        public IViewComponentResult Invoke()
        {
            var menuItems = this.pagesRepository
                .All()
                .Where(p => !p.IsDeleted)
                .To<MenuItemViewModel>()
                .ToList();

            return this.View(menuItems);
        }
    }
}
