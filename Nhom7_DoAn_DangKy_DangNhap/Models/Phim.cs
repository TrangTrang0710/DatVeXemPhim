using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class Phim
    {
        [Key]
        public string MaPhim { get; set; }= string.Empty;

        [Required]
        [StringLength(100)]
        public string TenPhim { get; set; }= string.Empty;

        public string TheLoai { get; set; }= string.Empty;
        public int? ThoiLuong { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public string DaoDien { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? NgayKhoiChieu { get; set; }= DateTime.Now;

        [Required]
        public string TenDangNhap { get; set; }= string.Empty;

        [ForeignKey("TenDangNhap")]
        public TaiKhoan? TaiKhoan { get; set; } 

        public ICollection<SuatChieu> SuatChieus { get; set; } = new List<SuatChieu>();
        [Display(Name = "Ảnh poster phim")]
        public string Anh { get; set; } = "/images/no-poster.jpg"; // hoặc "" nếu bạn muốn kiểm tra null

        
        
    }
}
