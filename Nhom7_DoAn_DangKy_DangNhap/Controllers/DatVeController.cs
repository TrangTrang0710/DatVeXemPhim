using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using Nhom7_DoAn_DangKy_DangNhap.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class DatVeController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;
        private readonly EmailService _emailService;

        private const string SessionOtp = "_Otp";
        private const string SessionEmail = "_Email";
        private const string SessionGhe = "_Ghe";
        private const string SessionSuat = "_Suat";

        public DatVeController(Nhom7_DoAn_DangKy_DangNhapContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ✅ 1. Hiển thị danh sách phim
        public async Task<IActionResult> ChonPhim()
        {
            var phimList = await _context.Phim.ToListAsync();
            return View(phimList);
        }

        // ✅ 2. Hiển thị suất chiếu theo phim
        public async Task<IActionResult> ChonSuatChieu(string maPhim, string maPhong)
        {
            if (string.IsNullOrEmpty(maPhim) || string.IsNullOrEmpty(maPhong))
                return RedirectToAction("ChonPhong");

            var suatChieu = await _context.SuatChieu
                .Include(sc => sc.PhongChieu)
                .Where(sc => sc.MaPhim == maPhim && sc.MaPhongChieu == maPhong)
                .OrderBy(sc => sc.NgayChieu)
                .ThenBy(sc => sc.ThoiGianChieu)
                .ToListAsync();

            ViewBag.Phim = await _context.Phim.FindAsync(maPhim);
            ViewBag.Phong = await _context.PhongChieu.FindAsync(maPhong);
            return View(suatChieu);
        }
        // chọn phòng chiếu theo phim
        public async Task<IActionResult> ChonPhong(string maPhim)
        {
            if (string.IsNullOrEmpty(maPhim))
                return RedirectToAction("ChonPhim"); // fallback an toàn

            var phongChieu = await _context.SuatChieu
                .Include(s => s.PhongChieu)
                .Where(s => s.MaPhim == maPhim)
                .Select(s => s.PhongChieu)
                .Distinct()
                .ToListAsync();

            ViewBag.Phim = await _context.Phim.FindAsync(maPhim);

            return View(phongChieu);
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
                .OrderBy(g => g.TenGhe) // Đảm bảo sắp xếp
                .ToListAsync();

            // Danh sách ghế đã đặt
            var gheDaDat = await _context.Ve
                .Where(v => v.MaSuatChieu == maSuatChieu && v.TrangThai == "Đã thanh toán")
                .Select(v => v.TenGhe)
                .ToListAsync();

            // Tạo Dictionary<string, List<Ghe>> để render theo hàng
            var gheTheoHang = gheList
                .GroupBy(g => g.TenGhe.Substring(0, 1)) // A1 => A
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.OrderBy(ghe => Convert.ToInt32(ghe.TenGhe.Substring(1))).ToList());

            ViewBag.SuatChieu = suatChieu;
            ViewBag.GheDaDat = gheDaDat;
            ViewBag.GheTheoHang = gheTheoHang;

            return View();
        }


        // ✅ 4. Hiển thị form thanh toán (nhận danh sách ghế đã chọn)
        [HttpPost]
        public IActionResult ThanhToan(string[] gheDaChon, string maSuatChieu)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
                return RedirectToAction("ChonGhe", new { maSuatChieu });

            var suat = _context.SuatChieu
                               .Include(sc => sc.Phim)
                               .Include(sc => sc.PhongChieu)
                               .FirstOrDefault(sc => sc.MaSuatChieu == maSuatChieu);
            if (suat == null)
                return NotFound("Suất chiếu không tồn tại");

            var gheList = _context.Ghe
                .Include(g => g.LoaiGhe)
                .Where(g => gheDaChon.Contains(g.TenGhe) && g.MaPhongChieu == suat.MaPhongChieu)
                .ToList();

            // Tính tiền và tạo danh sách chi tiết
            decimal giaVe = suat.GiaVe;
            decimal tongTien = giaVe * gheList.Count;

            var chiTietGhe = new List<(string TenGhe, string LoaiGhe, decimal Gia)>();

            foreach (var ghe in gheList)
            {
                string loai = ghe.MaLoaiGhe switch
                {
                    "LG01" => "Ghế thường",
                    "LG02" => "Ghế VIP",
                    "LG03" => "Ghế Couple",
                    _ => "Không xác định"
                };

                chiTietGhe.Add((ghe.TenGhe, loai, giaVe));
            }

            ViewBag.Suat = suat;
            ViewBag.ChiTietGhe = chiTietGhe;
            ViewBag.TongTien = tongTien;

            return View();
        }

        // ✅ Gửi OTP đến email
        [HttpPost]
        public async Task<IActionResult> GuiOTP(string email, string[] gheDaChon, string maSuatChieu, decimal tongTien)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Email không được bỏ trống.";
                return View("ThanhToan");
            }

            var otp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString(SessionOtp, otp);
            HttpContext.Session.SetString(SessionEmail, email);
            HttpContext.Session.SetString(SessionGhe, string.Join("|", gheDaChon));
            HttpContext.Session.SetString(SessionSuat, maSuatChieu);
            HttpContext.Session.SetString("TongTien", tongTien.ToString());

            await _emailService.SendEmailAsync(email, "Xác nhận OTP đặt vé", $"Mã OTP của bạn là: {otp}");

            TempData["Message"] = "✅ Mã OTP đã được gửi đến email của bạn!";
            return RedirectToAction("NhapOTP");
        }


        [HttpGet]
        public IActionResult NhapOTP()
        {
            ViewBag.Message = TempData["Message"];

            var maSuatChieu = HttpContext.Session.GetString(SessionSuat);
            var gheDaChon = HttpContext.Session.GetString(SessionGhe)?.Split('|') ?? new string[0];
            var tongTienStr = HttpContext.Session.GetString("TongTien");
            decimal tongTien = 0;
            decimal.TryParse(tongTienStr, out tongTien);

            var suat = _context.SuatChieu
                               .Include(s => s.Phim)
                               .Include(s => s.PhongChieu)
                               .FirstOrDefault(sc => sc.MaSuatChieu == maSuatChieu);

            var chiTietGhe = new List<(string TenGhe, string LoaiGhe, decimal Gia)>();

            foreach (var tenGhe in gheDaChon)
            {
                var ghe = _context.Ghe.FirstOrDefault(g => g.TenGhe == tenGhe && g.MaPhongChieu == suat!.MaPhongChieu);
                if (ghe != null)
                {
                    decimal gia = suat!.GiaVe;
                    string loai = ghe.MaLoaiGhe switch
                    {
                        "LG01" => "Ghế thường",
                        "LG02" => "Ghế VIP",
                        "LG03" => "Ghế Couple",
                        _ => "Không xác định"
                    };

                    chiTietGhe.Add((tenGhe, loai, gia));
                }
            }

            ViewBag.Suat = suat;
            ViewBag.ChiTietGhe = chiTietGhe;
            ViewBag.TongTien = tongTien;

            return View();
        }


        // ✅ Xác nhận OTP và tạo vé
        [HttpPost]
        public IActionResult XacNhanOTP(string otp)
        {
            var otpSession = HttpContext.Session.GetString(SessionOtp);
            if (otp != otpSession)
            {
                ViewBag.Error = "❌ Mã OTP không chính xác!";
                return View("NhapOTP");
            }

            var email = HttpContext.Session.GetString(SessionEmail);
            var gheDaChon = HttpContext.Session.GetString(SessionGhe)?.Split('|') ?? new string[0];
            var maSuatChieu = HttpContext.Session.GetString(SessionSuat);
            var tongTienStr = HttpContext.Session.GetString("TongTien");

            decimal tongTien = 0;
            decimal.TryParse(tongTienStr, out tongTien);

            var suat = _context.SuatChieu
                               .Include(s => s.Phim)
                               .Include(s => s.PhongChieu)
                               .FirstOrDefault(sc => sc.MaSuatChieu == maSuatChieu);

            var danhSachVe = new List<Ve>();

            foreach (var ghe in gheDaChon)
            {
                var ve = new Ve
                {
                    MaVe = Guid.NewGuid().ToString(),
                    MaNguoiDung = "ND01", // Có thể sửa lại thành người dùng đăng nhập thực tế
                    MaSuatChieu = maSuatChieu!,
                    TenGhe = ghe,
                    MaPhongChieu = suat!.MaPhongChieu,
                    NgayDat = DateTime.Now,
                    TrangThai = "Đã thanh toán"
                };
                danhSachVe.Add(ve);
                _context.Ve.Add(ve);
            }

            _context.SaveChanges();

            // ❌ Không cần tính lại tiền từ DB nữa

            // ✅ Xóa session sau khi hoàn tất
            HttpContext.Session.Clear();

            // ✅ Gửi thông tin sang View
            ViewBag.Email = email;
            ViewBag.Suat = suat;
            ViewBag.DanhSachVe = danhSachVe;
            ViewBag.TongTien = tongTien;

            return View("ThanhToanThanhCong");
        }



        public IActionResult ThanhToanThanhCong()
        {
            return View();
        }

    }
}
