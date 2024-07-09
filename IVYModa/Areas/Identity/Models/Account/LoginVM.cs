using System.ComponentModel.DataAnnotations;

namespace IVYModa.Areas.Identity.Models.Account
{
    public class LoginVM
    {
        [Display(Name = "Email hoặc Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string EmailOrPhoneNumber { get; set; } = null!;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 7, ErrorMessage = "Vui lòng nhập {0} độ dài từ {2} tới {1} ký tự")]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
