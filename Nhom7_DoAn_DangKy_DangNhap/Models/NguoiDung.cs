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

        // true: bị khóa, false: đang hoạt động
        [NotMapped]
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// Trạng thái tài khoản: "Bị khóa" hoặc "Đang hoạt động"
        /// Thuộc tính này không được lưu vào CSDL
        /// </summary>
        [NotMapped]
        public string TrangThai
        {
            get => IsLocked ? "Bị khóa" : "Đang hoạt động";
            set { } // cần có set để tránh lỗi khi binding ở View
        }

        /// <summary>
        /// Vai trò người dùng: Admin / Nhân viên / Khách hàng
        /// Xác định dựa trên thông tin tài khoản liên kết
        /// </summary>
        [NotMapped]
        public string VaiTro
        {
            get
            {
                if (TaiKhoan != null && TaiKhoan.TenDangNhap == "admin" && TaiKhoan.MatKhau == "1234")
                    return "Admin";
                else if (TaiKhoan != null)
                    return "Nhân viên";
                else
                    return "Khách hàng";
            }
            set { } // cần có set để tránh lỗi binding trong View
        }
    }
}

