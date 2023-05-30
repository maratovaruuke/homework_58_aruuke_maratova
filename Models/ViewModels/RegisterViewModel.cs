using homework_59_aruuke_maratova.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;

namespace homework_59_aruuke_maratova.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public IFormFile Avatar { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
        public string Bio { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
    }
}
