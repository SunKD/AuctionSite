using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private DashboardContext _context;

        public HomeController(DashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Password = model.Password
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);

                _context.Users.Add(NewUser);
                _context.SaveChanges();
                
                var loginUser = _context.Users.SingleOrDefault(User => User.UserName == model.UserName);
                HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);

                return RedirectToAction("Dashboard", "Auction");
            }
            return View("Login");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("loggingIn")]
        public IActionResult LoggingIn(string username, string loginpw)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            var loginUser = _context.Users.SingleOrDefault(User => User.UserName == username);
            if (loginUser != null)
            {
                var hashedPw = Hasher.VerifyHashedPassword(loginUser, loginUser.Password, loginpw);
                if (hashedPw == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);
                    return RedirectToAction("Dashboard", "Auction");
                }
            }

            ViewBag.Error = "Email address or Password is not matching";
            return View("Login");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
