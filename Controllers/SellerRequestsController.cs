using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using CampusMarket.Data;
using CampusMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusMarket.Controllers
{
    public class SellerRequestsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;


        public SellerRequestsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if(User.IsInRole("Seller") || User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var userId = _userManager.GetUserId(User);
            var lastReq = await _db.SellerRequests
                .Where(r => r.UserId == userId)
                 .OrderByDescending(r => r.RequestDate)
                 .FirstOrDefaultAsync();
            return View(lastReq);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string fullName, string phoneNumber, string reason)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Seller"))
            {
                RedirectToAction("Index", "Home");
            }

             var userId = _userManager.GetUserId(User);
            if(userId == null)
            {
                return Challenge();
            }
            if(string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(reason))
            {
                TempData["message"] = "Te rugam sa completezi toate campurile.";
                return RedirectToAction("Create");
            }

            var pending = await _db.SellerRequests
                .AnyAsync(r => r.UserId == userId && r.Status == "In asteptare");

            if(pending)
            {
                TempData["message"] = "Ai deja o cerere in asteptare.";
                return RedirectToAction("Create");
            }
            var request = new SellerRequest
            {
                UserId = userId,
                FullName = fullName.Trim(),
                PhoneNumber = phoneNumber.Trim(),
                Reason = reason.Trim(),
                Status = "In asteptare",
                RequestDate = DateTime.Now
            };
            _db.SellerRequests.Add(request);
            await _db.SaveChangesAsync();

            TempData["message"] = "Cererea a fost trimisa cu succes.";
            return RedirectToAction("Create");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var request = await _db.SellerRequests
                   .Include(r => r.User)
                   .OrderByDescending(r => r.RequestDate)
                   .ToListAsync();
            return View(request);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _db.SellerRequests.FindAsync(id);
            if(request == null || request.Status != "In asteptare")
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null || !await _userManager.IsInRoleAsync(user, "Seller")){
                await _userManager.AddToRoleAsync(user, "Seller");
            }
            request.Status = "Aprobata";
            await _db.SaveChangesAsync();

            TempData["message"] = "Cererea a fost aprobata.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject (int id)
        {
            var request = await _db.SellerRequests.FindAsync(id);
            if(request == null || request.Status != "In asteptare")
            {
                return NotFound();
            }
            request.Status = "Respins";
            await _db.SaveChangesAsync();

            TempData["message"] = "Cererea a fost respinsa.";
            return RedirectToAction("Index");

        }
       
    }
}
