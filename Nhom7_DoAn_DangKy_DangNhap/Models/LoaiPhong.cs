using System.ComponentModel.DataAnnotations;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class LoaiPhong
    {
        [Key]
        public string MaLoaiPhong { get; set; } = string.Empty;// 'Thuong', 'VIP', 'Couple'
        public int SoLuongGhe { get; set; } // Số lượng ghế trong phòng
        public ICollection<PhongChieu> PhongChieus { get; set; }

    }
}
