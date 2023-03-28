using Microsoft.AspNetCore.Mvc;

namespace NewsBlog.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Weather()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
