using PetAdoptionPortal.Data;
using PetAdoptionPortal.Filters;
using PetAdoptionPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PetAdoptionPortal.Controllers
{
    // Staff-only CRUD for a pet's vaccination / care records
    [RequireLogin]
    public class VaccinationsController : Controller
    {
        private readonly AppDbContext _db;

        public VaccinationsController(AppDbContext db)
        {
            _db = db;
        }

        // CREATE - show form for a given pet
        public async Task<IActionResult> Create(int petId)
        {
            var pet = await _db.Pets.FindAsync(petId);
            if (pet == null) return NotFound();

            ViewBag.PetName = pet.Name;
            return View(new VaccinationRecord { PetId = petId });
        }

        // CREATE - save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetId,VaccineName,DateGiven,NextDueDate,Notes")] VaccinationRecord record)
        {
            var pet = await _db.Pets.FindAsync(record.PetId);
            if (pet == null) return NotFound();

            if (record.NextDueDate != null && record.NextDueDate <= record.DateGiven)
            {
                ModelState.AddModelError("NextDueDate", "Next due date must be after the date given");
            }

            if (ModelState.IsValid)
            {
                _db.VaccinationRecords.Add(record);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Care record added.";
                return RedirectToAction("Details", "Pets", new { id = record.PetId });
            }

            ViewBag.PetName = pet.Name;
            return View(record);
        }

        // UPDATE - show form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var record = await _db.VaccinationRecords.Include(v => v.Pet).FirstOrDefaultAsync(v => v.Id == id);
            if (record == null) return NotFound();

            ViewBag.PetName = record.Pet?.Name;
            return View(record);
        }

        // UPDATE - save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PetId,VaccineName,DateGiven,NextDueDate,Notes")] VaccinationRecord record)
        {
            if (id != record.Id) return NotFound();

            var existing = await _db.VaccinationRecords.Include(v => v.Pet).FirstOrDefaultAsync(v => v.Id == id);
            if (existing == null) return NotFound();

            if (record.NextDueDate != null && record.NextDueDate <= record.DateGiven)
            {
                ModelState.AddModelError("NextDueDate", "Next due date must be after the date given");
            }

            if (ModelState.IsValid)
            {
                existing.VaccineName = record.VaccineName;
                existing.DateGiven = record.DateGiven;
                existing.NextDueDate = record.NextDueDate;
                existing.Notes = record.Notes;

                await _db.SaveChangesAsync();

                TempData["Success"] = "Care record updated.";
                return RedirectToAction("Details", "Pets", new { id = existing.PetId });
            }

            ViewBag.PetName = existing.Pet?.Name;
            return View(record);
        }

        // DELETE - the button on the pet page asks "Are you sure?" with JavaScript, then posts here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _db.VaccinationRecords.FindAsync(id);
            if (record == null) return NotFound();

            int petId = record.PetId;
            _db.VaccinationRecords.Remove(record);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Care record deleted.";
            return RedirectToAction("Details", "Pets", new { id = petId });
        }
    }
}
