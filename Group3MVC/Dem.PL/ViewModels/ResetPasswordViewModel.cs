using System.ComponentModel.DataAnnotations;

namespace Dem.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "New Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Comfirm New Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Doesn't Match")]
        public string ComfirmNewPassword { get; set; }
    }
}
