using System.ComponentModel.DataAnnotations;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class VerifyOTPViewModel
    {
        [Required(ErrorMessage = "OTP không được để trống")]
        public string OTP { get; set; } = string.Empty;
    }
}
