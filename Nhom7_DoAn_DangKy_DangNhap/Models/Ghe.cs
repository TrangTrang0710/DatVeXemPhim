using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class Ghe
    {
        public string TenGhe { get; set; }= string.Empty;
        public string MaPhongChieu { get; set; } = string.Empty;
        public string MaLoaiGhe { get; set; } = string.Empty;

        // Navigation Properties (tùy chọn nếu bạn có entity PhongChieu và LoaiGhe)
        public virtual PhongChieu? PhongChieu { get; set; }
        public virtual LoaiGhe? LoaiGhe { get; set; }
    }
}
