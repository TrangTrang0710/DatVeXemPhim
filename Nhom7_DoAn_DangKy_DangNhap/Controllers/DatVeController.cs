using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using Nhom7_DoAn_DangKy_DangNhap.Services;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
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
        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(string[] gheDaChon, string maSuatChieu, string email)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
                return BadRequest("Chưa chọn ghế.");

            // ✅ Sinh mã OTP
            string otp = new Random().Next(100000, 999999).ToString();
            TempData["OTP"] = otp;
            TempData["Email"] = email;
            TempData["GheDaChon"] = string.Join("|", gheDaChon);
            TempData["MaSuatChieu"] = maSuatChieu;

            // ✅ Gửi OTP
            await _emailService.SendEmailAsync(email, "Mã OTP Xác Nhận", $"Mã OTP của bạn là: {otp}");

            return View("NhapOTP");
        }


        [HttpPost]
        public async Task<IActionResult> GuiOTP(string email)
        {
            // Tạo mã OTP
            string otp = new Random().Next(100000, 999999).ToString();
            TempData["OTP"] = otp;
            TempData["Email"] = email;

            var message = new MailMessage("he_thong@gmail.com", email)
            {
                Subject = "Mã OTP xác nhận đặt vé",
                Body = $"Mã OTP của bạn là: {otp}",
                IsBodyHtml = false
            };

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("he_thong@gmail.com", "APP_PASSWORD_16_KY_TU");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }

            TempData.Keep("OTP");
            TempData.Keep("Email");
            TempData.Keep("GheDaChon");
            TempData.Keep("MaSuatChieu");

            return View("NhapOTP");
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanOTP(string maOTP)
        {
            var otp = TempData["OTP"] as string;
            var email = TempData["Email"] as string;
            var gheDaChon = (TempData["GheDaChon"] as string)?.Split('|');
            var maSuatChieu = TempData["MaSuatChieu"] as string;

            if (maOTP == otp)
            {
                // ✅ Lưu vé vào DB
                foreach (var ghe in gheDaChon!)
                {
                    var ve = new Ve
                    {
                        MaVe = Guid.NewGuid().ToString(),
                        MaNguoiDung = "ND01", // Tạm cứng
                        MaSuatChieu = maSuatChieu!,
                        TenGhe = ghe,
                        NgayDat = DateTime.Now,
                        TrangThai = "Đã thanh toán"
                    };
                    _context.Ve.Add(ve);
                }
                await _context.SaveChangesAsync();

                return View("ThanhToanThanhCong");
            }

            ViewBag.ThongBao = "❌ Mã OTP không đúng.";
            TempData.Keep();
            return View("NhapOTP");
        }





        public IActionResult ThongBaoThanhToanThanhCong()
        {
            return View();
        }
    }
}
