using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class TaiKhoan
    {
        [Key]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(25, ErrorMessage = "Tên đăng nhập tối đa 25 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Mật khẩu phải từ 3 ký tự trở lên")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        public string VaiTro { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Trạng thái tài khoản")]
        public string TrangThaiTK { get; set; } = string.Empty;

        [ForeignKey("MaNguoiDung")]
        public string MaNguoiDung { get; set; } = string.Empty;
        public NguoiDung? NguoiDung { get; set; }
        public bool IsLocked { get; set; } = false;
    }
}

