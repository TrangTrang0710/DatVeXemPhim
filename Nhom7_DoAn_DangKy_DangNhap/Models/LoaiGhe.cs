using System.ComponentModel.DataAnnotations;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class LoaiGhe
    {
        [Key]
        public string MaLoaiGhe { get; set; } = string.Empty; 
        public string TenLoaiGhe { get; set; } = string.Empty; // Ghế thường, VIP, Couple
        public ICollection<Ghe>? Ghes { get; set; }

    }
}
