using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using Nhom7_DoAn_DangKy_DangNhap.Services;
using System.Security.Claims;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class DatVeController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;
        private readonly EmailService _emailService;
        public DatVeController(Nhom7_DoAn_DangKy_DangNhapContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ChonSuatChieu", "DatVe");
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
                return BadRequest("Mã suất chiếu không được để trống.");

            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .FirstOrDefaultAsync(s => s.MaSuatChieu == maSuatChieu);

            if (suatChieu == null)
                return NotFound("Không tìm thấy suất chiếu.");

            var gheList = await _context.Ghe
                .Where(g => g.MaPhongChieu == suatChieu.MaPhongChieu)
                .ToListAsync();

            // ✅ Lấy danh sách ghế đã được đặt cho suất chiếu này
            var gheDaDat = await _context.Ve
                .Where(v => v.MaSuatChieu == maSuatChieu && v.TrangThai == "Đã thanh toán")
                .Select(v => v.TenGhe)
                .ToListAsync();

            ViewBag.SuatChieu = suatChieu;
            ViewBag.GheDaDat = gheDaDat; // Truyền sang View

            return View(gheList);
        }


        // ✅ Hiển thị form thanh toán
        [Authorize]
        [HttpPost]
        public IActionResult ThanhToanForm(string[] gheDaChon, string maSuatChieu, string phuongThuc)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
                return BadRequest("Bạn chưa chọn ghế nào!");

            var suat = _context.SuatChieu
                .Include(s => s.Phim)
                .FirstOrDefault(s => s.MaSuatChieu == maSuatChieu);

            ViewBag.Suat = suat;
            ViewBag.Ghe = gheDaChon;
            ViewBag.PhuongThuc = phuongThuc;

            return View("ThanhToan");
        }

        // ✅ Xác nhận thanh toán -> tạo vé + gửi OTP
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(string[] gheDaChon, string maSuatChieu, string phuongThuc)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
                return BadRequest("Bạn chưa chọn ghế nào!");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("DangNhap", "TaiKhoans");

            var suat = await _context.SuatChieu.FirstOrDefaultAsync(s => s.MaSuatChieu == maSuatChieu);
            if (suat == null)
                return NotFound("Không tìm thấy suất chiếu.");

            var danhSachGhe = await _context.Ghe.Where(g => gheDaChon.Contains(g.TenGhe)).ToListAsync();
            var danhSachMaVe = new List<string>();

            foreach (var ghe in danhSachGhe)
            {
                var ve = new Ve
                {
                    MaVe = Guid.NewGuid().ToString(),
                    MaSuatChieu = suat.MaSuatChieu,
                    TenGhe = ghe.TenGhe,
                    MaPhongChieu = ghe.MaPhongChieu,
                    MaNguoiDung = userId,
                    TrangThai = "Chờ OTP"
                };

                _context.Ve.Add(ve);
                danhSachMaVe.Add(ve.MaVe);
            }

            await _context.SaveChangesAsync();

            // ✅ Sinh mã OTP và lưu Session
            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("DanhSachMaVe", string.Join(",", danhSachMaVe));

            // ✅ Giả lập gửi OTP (console)
            Console.WriteLine($"OTP của bạn: {otp}");

            // ✅ Gửi OTP qua Email thực tế
            var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.MaNguoiDung == userId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Mã OTP xác thực",
                    $"Xin chào {user.HoTen},<br/><br/>Mã OTP của bạn là: <b>{otp}</b>.<br/>Vui lòng nhập mã để hoàn tất thanh toán."
                );
            }
            return View("NhapOTP");
        }

        // ✅ Kiểm tra OTP
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> XacNhanOTP(string otpInput)
        {
            var otp = HttpContext.Session.GetString("OTP");
            var danhSachMaVe = HttpContext.Session.GetString("DanhSachMaVe")?.Split(",");

            if (otpInput == otp && danhSachMaVe != null)
            {
                var veList = await _context.Ve.Where(v => danhSachMaVe.Contains(v.MaVe)).ToListAsync();
                foreach (var ve in veList)
                {
                    ve.TrangThai = "Đã thanh toán";
                }
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("DanhSachMaVe");

                return View("ThanhCong");
            }

            ViewBag.Error = "OTP không đúng, vui lòng thử lại.";
            return View("NhapOTP");
        }
    }
}
