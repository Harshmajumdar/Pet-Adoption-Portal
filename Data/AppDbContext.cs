using PetAdoptionPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace PetAdoptionPortal.Data
{
    // The DbContext is the bridge between our C# classes and the MySQL database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<AdoptionApplication> AdoptionApplications { get; set; }
        public DbSet<VaccinationRecord> VaccinationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Deleting a pet also deletes its vaccination records
            modelBuilder.Entity<VaccinationRecord>()
                .HasOne(v => v.Pet).WithMany(p => p.VaccinationRecords)
                .HasForeignKey(v => v.PetId).OnDelete(DeleteBehavior.Cascade);

            // A pet that has adoption applications cannot be deleted
            modelBuilder.Entity<AdoptionApplication>()
                .HasOne(a => a.Pet).WithMany(p => p.Applications)
                .HasForeignKey(a => a.PetId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
