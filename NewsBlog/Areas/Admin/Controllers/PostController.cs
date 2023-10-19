using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Data;
using NewsBlog.Models;
using NewsBlog.ViewModels;

namespace NewsBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly AppDbContext _db;
        private readonly INotyfService _notification;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        public PostController(AppDbContext db, INotyfService notyfService, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _db = db;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(createPostViewModel);
            }

            //get logged in user's id
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);


            var post = new Post();
            post.Title = createPostViewModel.Title;
            post.Summary = createPostViewModel.Summary;
            post.Description = createPostViewModel.Description;
            post.UserId = user!.Id;

            if(post.Title != null)
            {
                string slug = createPostViewModel.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug +  "-" + Guid.NewGuid();
            }

            if(createPostViewModel.UploadImage != null)
            {
                post.ImageUrl = Image(createPostViewModel.UploadImage);
            }

            await _db.Posts!.AddAsync(post);
            await _db.SaveChangesAsync();
            _notification.Success("Blog created successfully");

            return RedirectToAction("Index");
        }

        private string Image(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString()+ "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using(FileStream fileStream = System.IO.File.OpenWrite(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
