using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVYModa.EF
{
    [Table("LoginLogs")]
    public class LoginLog
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Device { get; set; } = null!;

        [StringLength(50)]
        public string Software { get; set; } = null!;

        [StringLength(50)]
        public string LoginStyle { get; set; } = null!;

        [StringLength(250)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string IP { get; set; } = null!;

        [StringLength(50)]
        public string LoginTime { get; set; } = null!;

        public string IdUser { get; set; } = null!;

        public AppUser IdUserNav { get; set; } = null!;
    }
}
