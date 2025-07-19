using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class NguoiDung
    {
        [Key]
        public string MaNguoiDung { get; set; }= Guid.NewGuid().ToString();

        [Required]
        public string HoTen { get; set; }= string.Empty;

        [Required]
        public string Email { get; set; }= string.Empty;

        [Required]
        public string SDT { get; set; }= string.Empty;

        public TaiKhoan? TaiKhoan { get; set; }
        public ICollection<Ve>? Ves { get; set; }

    }
}
