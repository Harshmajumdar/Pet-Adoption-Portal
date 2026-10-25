using System.ComponentModel.DataAnnotations;

namespace PetAdoptionPortal.Models
{
    // One row = one person asking to adopt one pet
    public class AdoptionApplication
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        [Required(ErrorMessage = "Your name is required")]
        [StringLength(100)]
        [Display(Name = "Your Name")]
        public string ApplicantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        [StringLength(10)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please tell us why you want to adopt")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Please write between 10 and 500 characters")]
        [Display(Name = "Why do you want to adopt this pet?")]
        public string Reason { get; set; } = string.Empty;

        [Display(Name = "I already have other pets at home")]
        public bool HasOtherPets { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = ApplicationStatus.Pending;

        [Display(Name = "Applied On")]
        public DateTime AppliedOn { get; set; } = DateTime.Now;
    }
}
