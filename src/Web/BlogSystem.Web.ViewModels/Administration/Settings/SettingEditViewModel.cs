namespace BlogSystem.Web.ViewModels.Administration.Settings
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class SettingEditViewModel : SettingInputModel, IMapFrom<Setting>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
