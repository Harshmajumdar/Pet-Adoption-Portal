using System.ComponentModel.DataAnnotations;

namespace PetAdoptionPortal.Models
{
    // ViewModel = a model used only by a page (not saved in the database)
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
