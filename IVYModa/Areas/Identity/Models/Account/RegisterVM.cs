using IVYModa.EF;
using System.ComponentModel.DataAnnotations;

namespace IVYModa.Areas.Identity.Models.Account
{
    public class RegisterVM
    {
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string LastName { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "{0} không hợp lệ")]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "Ngày sinh nhật")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string BirthDay { get; set; } = null!;
        [Display(Name = "Giới tính")]
        public Genders Gender { get; set; }
        [Display(Name = "Tỉnh/TP")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public int CityCode { get; set; }
        [Display(Name = "Quận/Huyện")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public int DistrictCode { get; set; }
        [Display(Name = "Phường/Xã")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public int WardCode { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Address { get; set; } = null!;
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 7, ErrorMessage = "Vui lòng nhập {0} độ dài từ {2} tới {1} ký tự")]
        public string Password { get; set; } = null!;
        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Vui lòng {0}")]
        [Compare("Password", ErrorMessage = "Mật khẩu lặp lại không chính xác")]
        public string ConfirmPassword { get; set; } = null!;
        public bool CustomerAgree { get; set; }
        public bool CustomerSubscribe { get; set; }
    }
}
