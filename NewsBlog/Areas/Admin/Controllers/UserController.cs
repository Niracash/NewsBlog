using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using NewsBlog.Utilities;
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var viewModel = users.Select(x => new UserViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName
            }).ToList();
            return View(viewModel);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel); 
            }
            var checkUserEmail = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if(checkUserEmail!= null)
            {
                _notification.Error("Email already exists");
                return View(registerViewModel);
            }
            var checkUsername = await _userManager.FindByNameAsync(registerViewModel.UserName);
            if(checkUsername != null)
            {
                _notification.Error("Username is taken");
                return View(registerViewModel);
            }

            var newUser = new User()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.UserName,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName
            };

            var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (result.Succeeded)
            {
                if (registerViewModel.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.Author);
                }
                _notification.Success("User is created!");
                return RedirectToAction("Index", "User", new {area = "Admin"});
            }
            return View(registerViewModel);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginViewModel());
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
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
        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("Logged out");
            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}
