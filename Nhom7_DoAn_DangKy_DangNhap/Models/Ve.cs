using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class Ve
    {
        [Key]
        public string MaVe { get; set; }= string.Empty;

        public string MaNguoiDung { get; set; } = string.Empty;
        public string MaSuatChieu { get; set; } = string.Empty;
        public string TenGhe { get; set; } = string.Empty;
        public string MaPhongChieu { get; set; } = string.Empty;

        public DateTime NgayDat { get; set; } =  DateTime.Now;
        public string TrangThai { get; set; } = string.Empty;

        [ForeignKey("MaNguoiDung")]
        public NguoiDung? NguoiDung { get; set; } 

        [ForeignKey("MaSuatChieu")]
        public SuatChieu? SuatChieu { get; set; }

        [ForeignKey("TenGhe, MaPhongChieu")]
        public Ghe? Ghe { get; set; }
    }
}
