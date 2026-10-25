namespace PetAdoptionPortal.Models
{
    // Fixed lists used in drop-downs and status fields
    public static class PetOptions
    {
        public static readonly string[] Species = { "Dog", "Cat", "Rabbit", "Bird", "Other" };
        public static readonly string[] Genders = { "Male", "Female" };
    }

    public static class PetStatus
    {
        public const string Available = "Available";
        public const string Adopted = "Adopted";
        public static readonly string[] All = { Available, Adopted };
    }

    public static class ApplicationStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
    }
}
