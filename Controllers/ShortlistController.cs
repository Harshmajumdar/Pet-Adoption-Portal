using PetAdoptionPortal.Data;
using PetAdoptionPortal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PetAdoptionPortal.Controllers
{
    // The visitor's "favourite pets" list. It is kept in the Session (state management),
    // so no login is needed and nothing is saved in the database.
    public class ShortlistController : Controller
    {
        private const string SessionKey = "Shortlist";
        private readonly AppDbContext _db;

        public ShortlistController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var ids = HttpContext.Session.GetObject<List<int>>(SessionKey) ?? new List<int>();

            var pets = await _db.Pets
                .Where(p => ids.Contains(p.Id))
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(pets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, string? returnUrl)
        {
            var petExists = await _db.Pets.AnyAsync(p => p.Id == id);
            if (petExists)
            {
                var ids = HttpContext.Session.GetObject<List<int>>(SessionKey) ?? new List<int>();
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                    HttpContext.Session.SetObject(SessionKey, ids);
                    TempData["Success"] = "Added to your shortlist.";
                }
            }

            return GoBack(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id, string? returnUrl)
        {
            var ids = HttpContext.Session.GetObject<List<int>>(SessionKey) ?? new List<int>();
            if (ids.Remove(id))
            {
                HttpContext.Session.SetObject(SessionKey, ids);
                TempData["Success"] = "Removed from your shortlist.";
            }

            return GoBack(returnUrl);
        }

        // Go back to the page the visitor came from (only if it is a page of our own site)
        private IActionResult GoBack(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
