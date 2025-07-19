using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class PhongChieu
    {
        [Key]
        public string MaPhongChieu { get; set; } = string.Empty;
        public string TenPhong { get; set; } = string.Empty;

        public string MaLoaiPhong { get; set; } = string.Empty;
        [ForeignKey("MaLoaiPhong")]
        public LoaiPhong? LoaiPhong { get; set; }
        public ICollection<Ghe>? Ghes { get; set;}
    }
}
