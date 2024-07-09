using System.ComponentModel.DataAnnotations;

namespace IVYModa.Areas.Identity.Models.Manage
{
    public class LocationVM
    {
        public string UserName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int CityCode { get; set; }

        public int DistrictCode { get; set; }

        public int WardCode { get; set; }

        public string Address { get; set; } = null!;

        public bool Default { get; set; }

        public string IdUser { get; set; } = null!;
    }
}
