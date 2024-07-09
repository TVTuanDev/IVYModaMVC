using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IVYModa.EF
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string UserName { get; set; } = null!;

        [StringLength(12)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public int CityCode { get; set; }

        [Required]
        public int DistrictCode { get; set; }

        [Required]
        public int WardCode { get; set; }

        public string Address { get; set; } = null!;

        public bool Default { get; set; }

        public string IdUser { get; set; } = null!;

        public AppUser IdUserNav { get; set; } = null!;
    }
}
