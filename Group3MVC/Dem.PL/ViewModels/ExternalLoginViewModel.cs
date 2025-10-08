using System.ComponentModel.DataAnnotations;

namespace Dem.PL.ViewModels
{
    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string ProviderDisplayName { get; set; } = string.Empty;
    }
}
