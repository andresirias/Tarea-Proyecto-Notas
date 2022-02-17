using SistemaNotas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SistemaNotas.Controllers
{
    public class NotasController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Nota Nota { get; set; }
        public NotasController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // return login view if user is not logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            // return login view if user is not logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            Nota = new Nota();
            if (id == null)
            {
                // create
                return View(Nota);
            }
            // update
            Nota = _db.Notas.FirstOrDefault(u => u.Id == id);
            if (Nota == null)
            {
                return NotFound();
            }
            return View(Nota);
        }

        public IActionResult View(int? id)
        {
            // return login view if user is not logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            Nota = _db.Notas.FirstOrDefault(u => u.Id == id);
            if (Nota == null)
            {
                return NotFound();
            }
            return View(Nota);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            // return login view if user is not logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }
            if (ModelState.IsValid)
            {
                if (Nota.Id == 0)
                {
                    // create
                    Nota.User = _db.Users.FirstOrDefault(u => u.Username == HttpContext.Session.GetString("Username"));
                    _db.Notas.Add(Nota);
                }
                else
                {
                    _db.Notas.Update(Nota);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Nota);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // return user notes
            var user = _db.Users.FirstOrDefault(u => u.Username == HttpContext.Session.GetString("Username"));
            return Json(new { data = await _db.Notas.Where(u => u.User.Username == user.Username).ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var notaFromDb = await _db.Notas.FirstOrDefaultAsync(u => u.Id == id);
            if (notaFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Notas.Remove(notaFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
