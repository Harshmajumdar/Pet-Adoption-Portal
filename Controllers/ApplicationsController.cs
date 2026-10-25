using PetAdoptionPortal.Data;
using PetAdoptionPortal.Filters;
using PetAdoptionPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PetAdoptionPortal.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly AppDbContext _db;

        public ApplicationsController(AppDbContext db)
        {
            _db = db;
        }

        // ---------- PUBLIC: visitors apply for a pet ----------

        // Show the application form
        public async Task<IActionResult> Apply(int petId)
        {
            var pet = await _db.Pets.FindAsync(petId);
            if (pet == null) return NotFound();

            if (pet.Status != PetStatus.Available)
            {
                TempData["Error"] = $"Sorry, {pet.Name} has already been adopted.";
                return RedirectToAction("Index", "Pets");
            }

            ViewBag.Pet = pet;
            return View(new AdoptionApplication { PetId = petId });
        }

        // Save the application
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply([Bind("PetId,ApplicantName,Email,Phone,City,Reason,HasOtherPets")] AdoptionApplication application)
        {
            var pet = await _db.Pets.FindAsync(application.PetId);
            if (pet == null) return NotFound();

            if (pet.Status != PetStatus.Available)
            {
                TempData["Error"] = $"Sorry, {pet.Name} has already been adopted.";
                return RedirectToAction("Index", "Pets");
            }

            // Same person cannot apply twice for the same pet while the first is pending
            bool duplicate = await _db.AdoptionApplications.AnyAsync(a =>
                a.PetId == application.PetId &&
                a.Email == application.Email &&
                a.Status == ApplicationStatus.Pending);

            if (duplicate)
            {
                ModelState.AddModelError("Email", "You already have a pending application for this pet.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Pet = pet;
                return View(application);
            }

            application.Status = ApplicationStatus.Pending;
            application.AppliedOn = DateTime.Now;
            _db.AdoptionApplications.Add(application);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Thank you! Your application for {pet.Name} was submitted. The shelter will contact you soon.";
            return RedirectToAction(nameof(Track), new { email = application.Email });
        }

        // Visitors check the status of their applications by email
        public async Task<IActionResult> Track(string? email)
        {
            var results = new List<AdoptionApplication>();

            if (!string.IsNullOrWhiteSpace(email))
            {
                results = await _db.AdoptionApplications
                    .Include(a => a.Pet)
                    .Where(a => a.Email == email)
                    .OrderByDescending(a => a.AppliedOn)
                    .ToListAsync();
            }

            ViewBag.Email = email;
            return View(results);
        }

        // ---------- STAFF ONLY: review applications ----------

        [RequireLogin]
        public async Task<IActionResult> Index(string? status)
        {
            var apps = _db.AdoptionApplications.Include(a => a.Pet).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                apps = apps.Where(a => a.Status == status);
            }

            ViewBag.Status = status;
            return View(await apps.OrderByDescending(a => a.AppliedOn).ToListAsync());
        }

        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var app = await _db.AdoptionApplications.Include(a => a.Pet).FirstOrDefaultAsync(a => a.Id == id);
            if (app == null || app.Pet == null) return NotFound();

            if (app.Status != ApplicationStatus.Pending)
            {
                TempData["Error"] = "This application has already been reviewed.";
                return RedirectToAction(nameof(Index));
            }

            if (app.Pet.Status == PetStatus.Adopted)
            {
                TempData["Error"] = $"{app.Pet.Name} is already adopted.";
                return RedirectToAction(nameof(Index));
            }

            app.Status = ApplicationStatus.Approved;
            app.Pet.Status = PetStatus.Adopted;

            // All other pending applications for the same pet are rejected automatically
            var others = await _db.AdoptionApplications
                .Where(a => a.PetId == app.PetId && a.Id != app.Id && a.Status == ApplicationStatus.Pending)
                .ToListAsync();
            foreach (var other in others)
            {
                other.Status = ApplicationStatus.Rejected;
            }

            await _db.SaveChangesAsync();

            TempData["Success"] = $"Approved! {app.Pet.Name} is now marked as Adopted.";
            return RedirectToAction(nameof(Index));
        }

        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var app = await _db.AdoptionApplications.FindAsync(id);
            if (app == null) return NotFound();

            if (app.Status != ApplicationStatus.Pending)
            {
                TempData["Error"] = "This application has already been reviewed.";
                return RedirectToAction(nameof(Index));
            }

            app.Status = ApplicationStatus.Rejected;
            await _db.SaveChangesAsync();

            TempData["Success"] = "Application rejected.";
            return RedirectToAction(nameof(Index));
        }
    }
}
