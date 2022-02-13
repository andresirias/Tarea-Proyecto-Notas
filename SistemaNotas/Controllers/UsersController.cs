using Microsoft.AspNetCore.Mvc;
using SistemaNotas.Models;

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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
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
            return RedirectToAction("Index");
        }
    }
}
