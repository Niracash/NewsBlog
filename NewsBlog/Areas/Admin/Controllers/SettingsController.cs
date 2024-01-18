using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NewsBlog.Data;
using NewsBlog.Models;
using NewsBlog.ViewModels;

namespace NewsBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly INotyfService _notification;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(AppDbContext db, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _db.Settings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // Optionally handle the case where settings are not found.
                // This should be rare as DataInitializer sets default settings.
                return View("Error"); // or a suitable response
            }

            var settingsViewModel = new SettingsViewModel()
            {
                Id = settings.Id,
                Name = settings.Name,
                Title = settings.Title,
                Description = settings.Description,
                ImageUrl = settings.ImageUrl,
                TwitterUrl = settings.TwitterUrl,
                FacebookUrl = settings.FacebookUrl,
                GithubUrl = settings.GithubUrl
            };

            return View(settingsViewModel);
        }

        // Edit
        [HttpPost]
        public async Task<IActionResult> Index(SettingsViewModel settingsViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(settingsViewModel);
            }
            var settings = await _db.Settings!.FirstOrDefaultAsync(x => x.Id == settingsViewModel.Id);
            if (settings == null)
            {
                _notification.Error("Error 404 not found");
                return View(settingsViewModel);
            }
            settings.Name = settingsViewModel.Name;
            settings.Title = settingsViewModel.Title;
            settings.Description = settingsViewModel.Description;
            settings.TwitterUrl = settingsViewModel.TwitterUrl;
            settings.FacebookUrl = settingsViewModel.FacebookUrl;
            settings.GithubUrl = settingsViewModel.GithubUrl;
            if (settingsViewModel.UploadImage != null)
            {
                settings.ImageUrl = Image(settingsViewModel.UploadImage);
            }
            await _db.SaveChangesAsync();
            _notification.Success("Settings updated!");
            return RedirectToAction("Index", "Settings", new { area = "Admin" });
        }

        private string Image(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.OpenWrite(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
