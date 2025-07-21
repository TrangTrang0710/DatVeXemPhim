using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class NguoiDung
    {
        [Key]
        public string MaNguoiDung { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string SDT { get; set; } = string.Empty;

        // Liên kết với tài khoản
        public TaiKhoan? TaiKhoan { get; set; }

        public ICollection<Ve>? Ves { get; set; }

        [NotMapped]
        public string Role
        {
            get
            {
                if (TaiKhoan != null && TaiKhoan.TenDangNhap == "admin" && TaiKhoan.MatKhau == "1234")
                    return "Admin";
                else if (TaiKhoan != null)
                    return "NhanVien";
                else
                    return "KhachHang";
            }
            set { }
        }

    }
}

