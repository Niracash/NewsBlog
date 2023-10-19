
namespace NewsBlog.ViewModels
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? UploadImage { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }

    }
}