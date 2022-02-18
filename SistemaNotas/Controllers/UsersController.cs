using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaNotas.Models;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace SistemaNotas.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public new User User { get; set; }
        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        private static string Hash(byte[] input, string algorithm = "sha256")
        {
            using (var hashAlgorithm = HashAlgorithm.Create(algorithm))
            {
                return Convert.ToBase64String(hashAlgorithm.ComputeHash(input));
            }
        }

        public IActionResult Register()
        {
            ViewBag.Title = "Registrarse";
            return View("Auth");
        }
        public IActionResult Login()
        {
            ViewBag.Title = "Ingresar";
            return View("Auth");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // hash password
                user.Password = Hash(System.Text.Encoding.UTF8.GetBytes(user.Password));
                _db.Users.Add(user);
                try
                {
                    _db.SaveChanges();
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Register");
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var userExists = _db.Users.Where(u => u.Username == user.Username).FirstOrDefault();
                if (userExists != null)
                {
                    // check if password is correct
                    if (userExists.Password == user.Password)
                    {
                        // create session
                        HttpContext.Session.SetString("Username", userExists.Username);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Login");
        }
    }
}