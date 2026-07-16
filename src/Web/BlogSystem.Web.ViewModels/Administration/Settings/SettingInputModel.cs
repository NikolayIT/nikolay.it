namespace BlogSystem.Web.ViewModels.Administration.Settings
{
    using System.ComponentModel.DataAnnotations;

    public class SettingInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
