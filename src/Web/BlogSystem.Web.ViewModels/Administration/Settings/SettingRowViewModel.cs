namespace BlogSystem.Web.ViewModels.Administration.Settings
{
    using System;

    using BlogSystem.Data.Models;
    using BlogSystem.Services.Mapping;

    public class SettingRowViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
