using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VitaBloom.Models;

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

        // =============================
        // ACCOUNT STORAGE
        // =============================

        private static string AccountFile =>
            Path.Combine(Directory.GetCurrentDirectory(), "accounts.json");

        private class Account
        {
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        private List<Account> GetAccounts()
        {
            if (!System.IO.File.Exists(AccountFile))
                return new List<Account>();

            string json = System.IO.File.ReadAllText(AccountFile);

            return JsonSerializer.Deserialize<List<Account>>(json)
                   ?? new List<Account>();
        }

        private void SaveAccounts(List<Account> accounts)
        {
            string json = JsonSerializer.Serialize(
                accounts,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            System.IO.File.WriteAllText(AccountFile, json);
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }


        // =============================
        // REGISTER
        // =============================

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var accounts = GetAccounts();

            // Check if email already exists
            if (accounts.Any(a =>
                a.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    "Email",
                    "This email is already registered.");

                return View(model);
            }

            // Save new account
            var account = new Account
            {
                FullName = model.Username,
                Email = model.Email,
                Password = HashPassword(model.Password)
            };

            accounts.Add(account);

            SaveAccounts(accounts);

            TempData["Message"] = "Account created successfully! You can now login.";

            return RedirectToAction("Login");
        }


        // =============================
        // LOGIN
        // =============================

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
            var accounts = GetAccounts();

            string hashedPassword = HashPassword(password);

            var account = accounts.FirstOrDefault(a =>
                a.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                && a.Password == hashedPassword);

            // Account found
            if (account != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.FullName),
            new Claim(ClaimTypes.Email, account.Email)
        };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction("Index");
            }

            // Account not found
            ViewBag.Error = "Invalid email or password.";

            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        public IActionResult Watertracker()
        {
            return View();
        }

        [Authorize]
        public IActionResult Recipes()
        {
            return View();
        }

        // =============================
        // LOGOUT
        // =============================

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
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
                // If an image was uploaded, we can set a placeholder path or
                // extend this to save the file to wwwroot/uploads and set ImagePath.
                if (post.ImageFile != null && post.ImageFile.Length > 0)
                {
                    // For now, do not attempt to save the file to avoid IO complexity here.
                    post.ImagePath = null;
                }

                // Add the post to the in-memory list and redirect to the Community view.
                posts.Insert(0, post);

                return RedirectToAction("Community");
            }

            // If the model is invalid, re-display the view with validation messages.
            return View(posts);
        }

        [Authorize]
        public IActionResult Settings()
        {
            return View();
        }
    }
}

