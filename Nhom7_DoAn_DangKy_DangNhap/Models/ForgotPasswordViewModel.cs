using System.ComponentModel.DataAnnotations;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        public string TenDangNhap { get; set; }
    }
}
