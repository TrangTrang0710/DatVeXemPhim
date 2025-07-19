using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class ThanhToan
    {
        [Key]
        public string MaThanhToan { get; set; }= string.Empty;
        public string MaVe { get; set; } = string.Empty;
        public DateTime NgayThanhToan { get; set; }= DateTime.Now;
        public decimal SoTien { get; set; }
        public string PhuongThuc { get; set; } = string.Empty;
        

        [ForeignKey("MaVe")]
        public Ve? Ve { get; set; }
    }
}
