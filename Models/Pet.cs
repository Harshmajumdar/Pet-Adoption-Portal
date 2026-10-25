using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetAdoptionPortal.Models
{
    // Model = one table in the database (table name: Pets)
    public class Pet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pet name is required")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choose a species")]
        [StringLength(20)]
        public string Species { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Breed { get; set; }

        [Range(0, 300, ErrorMessage = "Age must be between 0 and 300 months")]
        [Display(Name = "Age (in months)")]
        public int AgeInMonths { get; set; }

        [Required(ErrorMessage = "Please choose a gender")]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Enter a valid web address (starting with http)")]
        [StringLength(300)]
        [Display(Name = "Photo URL (optional)")]
        public string? ImageUrl { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = PetStatus.Available;

        [DataType(DataType.Date)]
        [Display(Name = "Added On")]
        public DateTime DateAdded { get; set; } = DateTime.Today;

        public List<VaccinationRecord> VaccinationRecords { get; set; } = new();
        public List<AdoptionApplication> Applications { get; set; } = new();

        // ---- Helpers (calculated, NOT stored in the database) ----

        [NotMapped]
        public string AgeText
        {
            get
            {
                if (AgeInMonths < 12) return AgeInMonths == 1 ? "1 month" : $"{AgeInMonths} months";
                int years = AgeInMonths / 12;
                return years == 1 ? "1 year" : $"{years} years";
            }
        }

        [NotMapped]
        public string Emoji => Species switch
        {
            "Dog" => "🐶",
            "Cat" => "🐱",
            "Rabbit" => "🐰",
            "Bird" => "🐦",
            _ => "🐾"
        };

        [NotMapped]
        public string ShortDescription =>
            string.IsNullOrEmpty(Description) ? "" :
            (Description.Length > 80 ? Description.Substring(0, 80) + "..." : Description);
    }
}
