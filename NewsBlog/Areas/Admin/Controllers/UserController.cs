using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using NewsBlog.ViewModels;

namespace NewsBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly INotyfService _notification;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, INotyfService notification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notification = notification;            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var findUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginViewModel.Username);
            if (findUser == null)
            {
                _notification.Error("Username not found");
                return View(loginViewModel);
            }
            var verifyPassword = await _userManager.CheckPasswordAsync(findUser, loginViewModel.Password);
            if(!verifyPassword)
            {
                _notification.Error("Wrong password");
                return View(loginViewModel);
            }
            await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, loginViewModel.RememberMe, true);
            _notification.Success("Logged in!");
            return RedirectToAction("Index", "User", new {area="Admin"});
        }
    }
}
