using Microsoft.AspNetCore.Mvc;

namespace NewsBlog.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
