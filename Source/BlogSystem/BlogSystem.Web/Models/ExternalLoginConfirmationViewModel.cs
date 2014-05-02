namespace BlogSystem.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}