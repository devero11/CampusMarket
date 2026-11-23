using CampusMarket.Data;
using CampusMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace CampusMarket.Controllers
{
    public class AdsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public AdsController(ApplicationDbContext db, UserManager<IdentityUser> userManager,IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> New()
        {
            Ad ad = new Ad();
            ad.Categ = await GetAllCategories();
            return View(ad);
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> New(Ad ad, IFormFile? image)
        {
            ModelState.Remove("SellerId");
            ModelState.Remove("ImageUrl");

            var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if(image == null || image.Length == 0)
            {
                ModelState.AddModelError("ImageUrl", "Imaginea este obligatorie.");

            }
            else
            {
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExt.Contains(ext))
                {
                    ModelState.AddModelError("ImageUrl", "Format inalid. Sunt permise doar : .jpeg, .jpg, .png, .webp");
                }
            }
            if(!ModelState.IsValid)
            {
                ad.Categ = await GetAllCategories();
                return View(ad);
            }

            var imagesFolder = Path.Combine(_env.WebRootPath, "images");
            Directory.CreateDirectory(imagesFolder);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image!.FileName).ToLowerInvariant();
            var filePath = Path.Combine(imagesFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream); 
            }


            ad.ImageUrl = "/images/" + fileName;
            ad.SellerId = _userManager.GetUserId(User)!;
            ad.Status = "In asteptare";
            ad.isActive = true;

            _db.Ads.Add(ad);
            await _db.SaveChangesAsync();
            TempData["message"] = "Anuntul a fost adaugat si este in asteptarea aprobarii administratorului";
            return RedirectToAction("MyAds");
        }


        [HttpGet]
        public async Task<IActionResult> MyAds()
        {
            var userId = _userManager.GetUserId(User);
            var ads = await _db.Ads
                .Include(a => a.Category)
                .Where(a => a.SellerId == userId)
                .OrderByDescending(a => a.Id)
                .ToListAsync();
            return View(ads);
        }

        [NonAction]
        public async Task<IEnumerable<SelectListItem>> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            foreach(var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
                

            }
            return selectList;
        }



    }
}
