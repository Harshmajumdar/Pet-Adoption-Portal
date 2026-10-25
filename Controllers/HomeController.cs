using PetAdoptionPortal.Data;
using PetAdoptionPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PetAdoptionPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        // Public home page
        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                AvailablePets = await _db.Pets.CountAsync(p => p.Status == PetStatus.Available),
                AdoptedPets = await _db.Pets.CountAsync(p => p.Status == PetStatus.Adopted),
                PendingApplications = await _db.AdoptionApplications.CountAsync(a => a.Status == ApplicationStatus.Pending),
                LatestPets = await _db.Pets
                    .Where(p => p.Status == PetStatus.Available)
                    .OrderByDescending(p => p.DateAdded).ThenByDescending(p => p.Id)
                    .Take(3)
                    .ToListAsync()
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
