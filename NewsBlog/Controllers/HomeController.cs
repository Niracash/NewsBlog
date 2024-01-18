using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Data;
using NewsBlog.Models;
using NewsBlog.ViewModels;
using System.Diagnostics;

namespace NewsBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;



        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            var settings = _db.Settings!.ToList();

            // Data from post
            homeViewModel.Title = settings[0].Title;
            homeViewModel.Description = settings[0].Description;
            homeViewModel.ImageUrl = settings[0].ImageUrl;
            homeViewModel.Posts = _db.Posts!.Include(x =>x.User).ToList();

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}