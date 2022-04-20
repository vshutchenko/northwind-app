using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindMvcApp.ViewModels.User
{
    public class UserInputViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Should contain at least 5 characters.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Entered passwords don't match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; }

        [Display(Name = "User info")]
        public string NorthwindDbId { get; set; }

        public IEnumerable<SelectListItem> Entities { get; set; }
    }
}
