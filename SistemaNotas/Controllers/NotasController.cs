using SistemaNotas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return View();
        }

        public IActionResult Upsert(int? id)
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Nota.Id == 0)
                {
                    // create
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
            return Json(new { data = await _db.Notas.ToListAsync() });
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
