using System.ComponentModel.DataAnnotations;

namespace PetAdoptionPortal.Models
{
    // One row = one vaccination / treatment given to a pet (the "Care" part of the portal)
    public class VaccinationRecord
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        [Required(ErrorMessage = "Vaccine / treatment name is required")]
        [StringLength(80)]
        [Display(Name = "Vaccine / Treatment")]
        public string VaccineName { get; set; } = string.Empty;

        // The DisplayFormat lines make the <input type="date"> show the saved date correctly
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Given")]
        public DateTime DateGiven { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Next Due Date (optional)")]
        public DateTime? NextDueDate { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }
    }
}
