namespace BlogSystem.Web.ViewModels.Home
{
    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class MenuItemViewModel : IMapFrom<Page>
    {
        public string Title { get; set; }

        public string Permalink { get; set; }
    }
}
