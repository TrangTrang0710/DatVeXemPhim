using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class DatVeController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public DatVeController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // ✅ Chọn suất chiếu
        public async Task<IActionResult> ChonSuatChieu(string maPhim)
        {
            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .Where(s => s.MaPhim == maPhim)
                .ToListAsync();

            ViewBag.Phim = await _context.Phim.FirstOrDefaultAsync(p => p.MaPhim == maPhim);
            return View(suatChieu);
        }

        // ✅ Chọn ghế
        
        public async Task<IActionResult> ChonGhe(string maSuatChieu)
        {
            if (string.IsNullOrEmpty(maSuatChieu))
            {
                return BadRequest("Mã suất chiếu không được để trống.");
            }
           
            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .FirstOrDefaultAsync(s => s.MaSuatChieu == maSuatChieu);
            if (suatChieu == null)
            {
                return NotFound("Không tìm thấy suất chiếu.");
            }

            var gheList = await _context.Ghe
                .Where(g => g.MaPhongChieu == suatChieu.MaPhongChieu)
                .ToListAsync();

            ViewBag.SuatChieu = suatChieu;
            return View(gheList);
        }

        // ✅ Xác nhận thanh toán
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ThanhToan(string[] gheDaChon, string maSuatChieu, string phuongThuc)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
                return BadRequest("Bạn chưa chọn ghế nào!");
            if (string.IsNullOrEmpty(phuongThuc))
                return BadRequest("Bạn chưa chọn phương thức thanh toán!");
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("DangNhap", "NguoiDung");
            var suat = await _context.SuatChieu
                .FirstOrDefaultAsync(s => s.MaSuatChieu == maSuatChieu);
            if (suat == null)
                return NotFound("Không tìm thấy suất chiếu.");

            var danhSachGhe = await _context.Ghe
                .Where(g => gheDaChon.Contains(g.TenGhe))
                .ToListAsync();

            var danhSachMaVe = new List<string>();

            foreach (var ghe in danhSachGhe)
            {
                decimal giaVe = suat.GiaVe;

                switch (ghe.MaLoaiGhe)
                {
                    case "LG02":
                        giaVe *= 1.5m;
                        break;
                    case "LG03":
                        giaVe *= 2m;
                        break;
                }

                var ve = new Ve
                {
                    MaVe = Guid.NewGuid().ToString(),
                    MaSuatChieu = suat.MaSuatChieu,
                    TenGhe = ghe.TenGhe,
                    MaPhongChieu = ghe.MaPhongChieu,
                    MaNguoiDung = userId,
                    TrangThai = "Chưa thanh toán"
                };

                _context.Ve.Add(ve);
                await _context.SaveChangesAsync(); 

               var thanhToan = new ThanhToan
                {
                    MaThanhToan = Guid.NewGuid().ToString(),
                    MaVe = ve.MaVe,
                    NgayThanhToan = DateTime.Now,
                    SoTien = giaVe, // ✅ đúng số tiền
                    PhuongThuc = phuongThuc
                };

                _context.ThanhToan.Add(thanhToan);

                danhSachMaVe.Add(ve.MaVe);
            }

            await _context.SaveChangesAsync();

            // ✅ Redirect đúng phương thức
            var veDauTien = danhSachMaVe.FirstOrDefault(); // chỉ dùng 1 mã để redirect

            if (phuongThuc == "MoMo")
                return RedirectToAction("ThanhToanOnline", "ThanhToans", new { maVe = veDauTien });

            if (phuongThuc == "VNPay")
                return RedirectToAction("ThanhToanVNPay", "ThanhToans", new { maVe = veDauTien });

            return RedirectToAction("XacNhan", new { maSuatChieu = maSuatChieu });
        }
    }
}

