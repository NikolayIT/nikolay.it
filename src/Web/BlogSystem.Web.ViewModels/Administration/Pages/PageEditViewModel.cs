namespace BlogSystem.Web.ViewModels.Administration.Pages
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class PageEditViewModel : PageInputModel, IMapFrom<Page>
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
