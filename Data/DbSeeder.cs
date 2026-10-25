using PetAdoptionPortal.Models;

namespace PetAdoptionPortal.Data
{
    // Adds sample data the first time the app runs (only if the Pets table is empty)
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Pets.Any()) return;

            var today = DateTime.Today;

            var bruno = new Pet { Name = "Bruno", Species = "Dog", Breed = "Labrador", AgeInMonths = 18, Gender = "Male", Description = "Friendly and energetic. Loves fetch and long walks. Good with children.", ImageUrl = "https://images.unsplash.com/photo-1552053831-71594a27632d?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-10) };
            var luna = new Pet { Name = "Luna", Species = "Cat", Breed = "Persian", AgeInMonths = 8, Gender = "Female", Description = "Calm, fluffy and loves to cuddle on the sofa.", ImageUrl = "https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-8) };
            var coco = new Pet { Name = "Coco", Species = "Rabbit", Breed = "Dutch", AgeInMonths = 6, Gender = "Female", Description = "Curious little bunny. Enjoys carrots and hopping around the garden.", ImageUrl = "https://images.unsplash.com/photo-1585110396000-c9ffd4e4b308?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-6) };
            var mango = new Pet { Name = "Mango", Species = "Bird", Breed = "Budgerigar", AgeInMonths = 24, Gender = "Male", Description = "Cheerful and chatty. Can already whistle a few tunes.", ImageUrl = "https://images.unsplash.com/photo-1552728089-57bdde30beb3?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-5) };
            var max = new Pet { Name = "Max", Species = "Dog", Breed = "Indian Pariah", AgeInMonths = 36, Gender = "Male", Description = "Loyal, healthy and street-smart. A perfect guard and a loving companion.", ImageUrl = "https://images.unsplash.com/photo-1583511655857-d19b40a7a54e?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-4) };
            var whiskers = new Pet { Name = "Whiskers", Species = "Cat", Breed = "Indian Domestic Shorthair", AgeInMonths = 4, Gender = "Female", Description = "Playful kitten who chases anything that moves.", ImageUrl = "https://images.unsplash.com/photo-1574158622682-e40e69881006?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-2) };
            var simba = new Pet { Name = "Simba", Species = "Cat", Breed = "Orange Tabby", AgeInMonths = 14, Gender = "Male", Description = "Found a loving home last month!", Status = PetStatus.Adopted, ImageUrl = "https://images.unsplash.com/photo-1533738363-b7f9aef128ce?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-40) };
            var bella = new Pet { Name = "Bella", Species = "Dog", Breed = "Golden Retriever", AgeInMonths = 14, Gender = "Female", Description = "Gentle, affectionate, and intelligent female Golden Retriever. Loves cuddling, playing fetch, and is wonderful with kids and families.", ImageUrl = "https://images.unsplash.com/photo-1587300003388-59208cc962cb?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-1) };
            var rocky = new Pet { Name = "Rocky", Species = "Dog", Breed = "Siberian Husky", AgeInMonths = 24, Gender = "Male", Description = "Majestic, intelligent, and loves outdoor activities. Friendly with other dogs.", ImageUrl = "https://images.unsplash.com/photo-1537151625747-768eb6cf92b2?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-3) };
            var milo = new Pet { Name = "Milo", Species = "Cat", Breed = "British Shorthair", AgeInMonths = 12, Gender = "Male", Description = "Gentle, affectionate, and has the softest coat. Loves snoozing on warm rugs.", ImageUrl = "https://images.unsplash.com/photo-1543852786-1cf6624b9987?auto=format&fit=crop&w=800&q=80", DateAdded = today.AddDays(-7) };

            db.Pets.AddRange(bruno, luna, coco, mango, max, whiskers, simba, bella, rocky, milo);
            db.SaveChanges();

            db.VaccinationRecords.AddRange(
                new VaccinationRecord { PetId = bruno.Id, VaccineName = "Rabies", DateGiven = today.AddDays(-60), NextDueDate = today.AddDays(305), Notes = "Annual booster" },
                new VaccinationRecord { PetId = bruno.Id, VaccineName = "DHPP", DateGiven = today.AddDays(-90), NextDueDate = today.AddDays(-5), Notes = "Booster is overdue" },
                new VaccinationRecord { PetId = luna.Id, VaccineName = "FVRCP", DateGiven = today.AddDays(-30), NextDueDate = today.AddDays(335) },
                new VaccinationRecord { PetId = max.Id, VaccineName = "Rabies", DateGiven = today.AddDays(-20), NextDueDate = today.AddDays(345) },
                new VaccinationRecord { PetId = max.Id, VaccineName = "Deworming", DateGiven = today.AddDays(-20), NextDueDate = today.AddDays(10), Notes = "Every 3 months" }
            );

            db.AdoptionApplications.Add(new AdoptionApplication
            {
                PetId = bruno.Id,
                ApplicantName = "Sneha Joshi",
                Email = "sneha@example.com",
                Phone = "9876501234",
                City = "Surat",
                Reason = "We have a big garden and a 10 year old who has wanted a dog for years.",
                HasOtherPets = false,
                AppliedOn = DateTime.Now.AddDays(-1)
            });

            db.SaveChanges();
        }
    }
}
