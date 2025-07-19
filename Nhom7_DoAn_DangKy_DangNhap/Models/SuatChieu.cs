

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class SuatChieu
    {
        [Key]
        public string MaSuatChieu { get; set; }= string.Empty;

        public string MaPhim { get; set; } = string.Empty;
        public string MaPhongChieu { get; set; } = string.Empty;

        public DateTime NgayChieu { get; set; }
        public TimeSpan ThoiGianChieu { get; set; }
        public decimal GiaVe { get; set; }

        [ForeignKey("MaPhim")]
        public Phim? Phim { get; set; }

        [ForeignKey("MaPhongChieu")]
        public PhongChieu? PhongChieu { get; set; }
        public ICollection<Ve>? Ves { get; set; }

    }
}
