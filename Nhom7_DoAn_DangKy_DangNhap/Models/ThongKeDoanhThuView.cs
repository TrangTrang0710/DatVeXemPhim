namespace Nhom7_DoAn_DangKy_DangNhap.Models
{
    public class ThongKeDoanhThuView
    {
        public string TenPhim { get; set; } = string.Empty;
        public decimal TongDoanhThu { get; set; }  // ✅ Sửa từ int → decimal
        
    }
}
