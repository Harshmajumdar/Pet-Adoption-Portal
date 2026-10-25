using PetAdoptionPortal.Data;
using PetAdoptionPortal.Filters;
using PetAdoptionPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PetAdoptionPortal.Controllers
{
    public class PetsController : Controller
    {
        private readonly AppDbContext _db;

        public PetsController(AppDbContext db)
        {
            _db = db;
        }

        private bool IsStaff => !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminName"));

        // READ (public) - browse pets with filter + search
        public async Task<IActionResult> Index(string? species, string? search)
        {
            var pets = _db.Pets.AsQueryable();

            // Visitors only see pets that are still available; staff see everything
            if (!IsStaff)
            {
                pets = pets.Where(p => p.Status == PetStatus.Available);
            }

            if (!string.IsNullOrEmpty(species))
            {
                pets = pets.Where(p => p.Species == species);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                pets = pets.Where(p => p.Name.Contains(search) || (p.Breed != null && p.Breed.Contains(search)));
            }

            ViewBag.SelectedSpecies = species;
            ViewBag.Search = search;

            return View(await pets.OrderByDescending(p => p.DateAdded).ThenBy(p => p.Name).ToListAsync());
        }

        // READ (public) - one pet with its vaccination records
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pet = await _db.Pets
                .Include(p => p.VaccinationRecords)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null) return NotFound();

            return View(pet);
        }

        // CREATE (staff only) - show form
        [RequireLogin]
        public IActionResult Create()
        {
            return View(new Pet());
        }

        // CREATE (staff only) - save form
        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Species,Breed,AgeInMonths,Gender,Description,ImageUrl")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                pet.Status = PetStatus.Available;
                pet.DateAdded = DateTime.Today;
                _db.Pets.Add(pet);
                await _db.SaveChangesAsync();

                TempData["Success"] = $"{pet.Name} was added to the portal!";
                return RedirectToAction(nameof(Index));
            }

            return View(pet);
        }

        // UPDATE (staff only) - show form
        [RequireLogin]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pet = await _db.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            return View(pet);
        }

        // UPDATE (staff only) - save changes
        [RequireLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Breed,AgeInMonths,Gender,Description,ImageUrl,Status")] Pet pet)
        {
            if (id != pet.Id) return NotFound();

            if (!PetStatus.All.Contains(pet.Status))
            {
                ModelState.AddModelError("Status", "Invalid status");
            }

            var existing = await _db.Pets.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.Name = pet.Name;
                existing.Species = pet.Species;
                existing.Breed = pet.Breed;
                existing.AgeInMonths = pet.AgeInMonths;
                existing.Gender = pet.Gender;
                existing.Description = pet.Description;
                existing.ImageUrl = pet.ImageUrl;
                existing.Status = pet.Status;

                await _db.SaveChangesAsync();

                TempData["Success"] = $"{existing.Name} was updated.";
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(pet);
        }

        // DELETE (staff only) - ask for confirmation
        [RequireLogin]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pet = await _db.Pets.FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null) return NotFound();

            return View(pet);
        }

        // DELETE (staff only) - really delete
        [RequireLogin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _db.Pets.FindAsync(id);
            if (pet == null) return NotFound();

            if (await _db.AdoptionApplications.AnyAsync(a => a.PetId == id))
            {
                TempData["Error"] = "Cannot delete: this pet has adoption applications. Mark it as Adopted instead.";
                return RedirectToAction(nameof(Index));
            }

            _db.Pets.Remove(pet);   // its vaccination records are deleted automatically
            await _db.SaveChangesAsync();

            TempData["Success"] = "Pet deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
