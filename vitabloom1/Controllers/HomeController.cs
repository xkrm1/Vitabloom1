using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VitaBloom.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VitaBloom.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // HOME
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // ABOUT
        public IActionResult About()
        {
            return View();
        }

        // CONTACT
        [Authorize]
        public IActionResult Contact()
        {
            return View();
        }

        // LOGIN
        private static List<RegisterViewModel> accounts = new List<RegisterViewModel>
{
    new RegisterViewModel
    {
        Username = "Admin",
        Email = "admin@gmail.com",
        Password = "123456",
        ConfirmPassword = "123456"
    }
};


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var account = accounts.FirstOrDefault(a =>
     a.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
     a.Password == password);

            if (account != null)
            {
                var claims = new List<Claim>
        {
           new Claim(ClaimTypes.Name, account.Username),
new Claim(ClaimTypes.Email, account.Email)
        };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToAction("Index");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Account created successfully!";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // DASHBOARD
        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        // WATER TRACKER
        public IActionResult WaterTracker()
        {
            return View();
        }

        // RECIPES
        [Authorize]
        public IActionResult Recipes()
        {
            return View();
        }

        // COMMUNITY
        
        private static List<CommunityPost> posts = new List<CommunityPost>
{
    new CommunityPost
    {
        Name = "Jane D.",
        Message = "Just finished my morning walk! 🚶",
        Likes = 24
    },

    new CommunityPost
    {
        Name = "Mark T.",
        Message = "Healthy lunch today! 🥗",
        Likes = 31
    },

    new CommunityPost
    {
        Name = "Rhea L.",
        Message = "Drinking 8 glasses of water daily 💧",
        Likes = 18
    }
};

        [HttpGet]
        public IActionResult Community()
        {
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Community(CommunityPost post)
        {
            if (ModelState.IsValid)
            {
                if (post.ImageFile != null && post.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads"
                    );

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(post.ImageFile.FileName);

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.ImageFile.CopyToAsync(stream);
                    }

                    post.ImagePath = "/uploads/" + fileName;
                }

                posts.Insert(0, post);
            }

            return View(posts);
        }


        // LOGOUT
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login", "Home");
        }
    }
}