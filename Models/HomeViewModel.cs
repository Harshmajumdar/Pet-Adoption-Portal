namespace PetAdoptionPortal.Models
{
    // ViewModel = a model made just for one page (not stored in the database)
    public class HomeViewModel
    {
        public int AvailablePets { get; set; }
        public int AdoptedPets { get; set; }
        public int PendingApplications { get; set; }
        public List<Pet> LatestPets { get; set; } = new();
    }
}
