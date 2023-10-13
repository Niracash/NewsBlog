using Microsoft.AspNetCore.Mvc;

namespace NewsBlog.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
