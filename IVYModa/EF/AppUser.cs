using Microsoft.AspNetCore.Identity;

namespace IVYModa.EF
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string BirthDay { get; set; } = null!;
        public Genders Gender { get; set; }

        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<LoginLog> LoginLogs { get; set; } = new List<LoginLog>();
    }

    public enum Genders
    {
        Male,
        Female,
        Other
    }
}
