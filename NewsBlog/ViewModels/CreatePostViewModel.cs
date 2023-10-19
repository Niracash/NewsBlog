
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.ViewModels
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? UploadImage { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Summary { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? UserId { get; set; }

    }
}