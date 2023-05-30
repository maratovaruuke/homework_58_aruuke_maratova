using System.ComponentModel.DataAnnotations;

namespace homework_59_aruuke_maratova.Models.ViewModels
{
    public class CreatePostViewModel
    {
        [Required]
        public IFormFile Img { get; set; }
        public string Caption { get; set; }
        public string UserId { get; set; }
    }
}
