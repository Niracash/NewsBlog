using NewsBlog.Models;

namespace NewsBlog.ViewModels
{
    public class HomeViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
