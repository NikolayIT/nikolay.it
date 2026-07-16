namespace BlogSystem.Web.ViewModels.Administration.Pages
{
    using System.ComponentModel.DataAnnotations;

    public class PageInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        [RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "The permalink can contain only letters, digits, dashes and underscores.")]
        public string Permalink { get; set; }

        [DataType(DataType.Html)]
        public string Content { get; set; }

        [Display(Name = "Meta description")]
        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        [Display(Name = "Meta keywords")]
        public string MetaKeywords { get; set; }
    }
}
