using IVYModa.EF;

namespace IVYModa.Areas.Identity.Models.Manage
{
    public class UserVM
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Genders Gender { get; set; }
        public string BirthDay { get; set; } = null!;
    }
}
