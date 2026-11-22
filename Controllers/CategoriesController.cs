using CampusMarket.Data;
using CampusMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusMarket.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoriesController (ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Show(int id )
        {
            Category? category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);

        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(Category cat)
        {
            if(!ModelState.IsValid)
            {
                return View(cat);
            }
            var name = cat.Name.Trim().ToLower();
            var exist = await _db.Categories
                .AnyAsync(c => c.Name.ToLower() == name);
            if (exist)
            {
                ModelState.AddModelError("Name", "Exista deja o categorie cu acest nume.");
                return View(cat);
            }


            try
            {
                cat.Name = cat.Name.Trim();                
                _db.Categories.Add(cat);
                await _db.SaveChangesAsync();
                TempData["message"] = "Categoria a fost introdusa cu succes.";
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return View(cat);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Category? cat = await _db.Categories.FindAsync(id);
            if (cat == null)
                return NotFound();
            return View(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category reqCat)
        {
            Category? cat = await _db.Categories.FindAsync(id);
            if (cat == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(reqCat);

            var name = reqCat.Name.Trim().ToLower();
            var exist = await _db.Categories
                .AnyAsync(c => c.Name.ToLower() == name);
            if (exist)
            {
                ModelState.AddModelError("Name", "Exista deja o categorie cu acest nume");
                return View(reqCat);
            }

            try
            {
                cat.Name = reqCat.Name.Trim();
                await _db.SaveChangesAsync();
                TempData["message"] = "Categoria o fost modificata cu succes";
                return RedirectToAction("Index");

            }
            catch(Exception)
            {
                return View(reqCat);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category? cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            _db.Categories.Remove(cat);
            await _db.SaveChangesAsync();
            TempData["message"] = "Categoria a fost stearsa";
            return RedirectToAction("Index"); 

        }

     }
}
