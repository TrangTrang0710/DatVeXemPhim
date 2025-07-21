using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;
        public ThongKeController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        // ✅ Thống kê vé bán (theo tổng số vé và tổng tiền)
        public IActionResult ThongKeVeBan()
        {
            var thongKe = _context.Ve
                .Where(ve => ve.TrangThai == "Đã thanh toán")
                .Join(_context.SuatChieu,
                      ve => ve.MaSuatChieu,
                      sc => sc.MaSuatChieu,
                      (ve, sc) => new { ve, sc })
                .Join(_context.Phim,
                      vs => vs.sc.MaPhim,
                      phim => phim.MaPhim,
                      (vs, phim) => new
                      {
                          phim.TenPhim,
                          vs.sc.GiaVe
                      })
                .GroupBy(x => x.TenPhim)
                .Select(g => new ThongKeVeBanView
                {
                    TenPhim = g.Key,
                    SoLuongVe = g.Count(),
                    TongDoanhThu = g.Sum(x => x.GiaVe)
                })
                .ToList();

            return View(thongKe);
        }

        // ✅ Thống kê doanh thu (theo thời gian)
        public IActionResult ThongKeDoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.Ve
                .Include(v => v.SuatChieu!)
                    .ThenInclude(sc => sc.Phim)
                .Where(v => v.TrangThai == "Đã thanh toán");

            if (tuNgay.HasValue && denNgay.HasValue)
            {
                query = query.Where(v => v.NgayDat.Date >= tuNgay.Value.Date && v.NgayDat.Date <= denNgay.Value.Date);
            }

            var thongKe = query
                .GroupBy(v => v.SuatChieu!.Phim!.TenPhim)
                .Select(g => new ThongKeDoanhThuView
                {
                    TenPhim = g.Key,
                    TongDoanhThu = g.Sum(v => v.SuatChieu!.GiaVe)
                })
                .ToList();

            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            return View(thongKe);
        }

    }
}
