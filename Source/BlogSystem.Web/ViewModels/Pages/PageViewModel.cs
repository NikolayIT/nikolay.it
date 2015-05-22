namespace BlogSystem.Web.ViewModels.Pages
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class PageViewModel : IMapFrom<Page>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime LastModified { get; set; }
    }
}