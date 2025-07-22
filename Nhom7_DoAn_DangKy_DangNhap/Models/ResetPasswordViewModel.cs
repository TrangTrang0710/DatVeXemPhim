using System.ComponentModel.DataAnnotations;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
