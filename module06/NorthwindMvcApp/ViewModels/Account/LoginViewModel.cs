using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
