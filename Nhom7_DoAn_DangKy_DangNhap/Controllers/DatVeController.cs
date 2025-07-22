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
        // Khai báo Session key
        private const string SessionOtpCode = "_OtpThanhToan";
        private const string SessionEmailTemp = "_EmailThanhToan";
        private const string SessionGheDaChon = "_GheDaChon";
        private const string SessionSuatChieu = "_MaSuatChieu";
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
        // ✅ Gửi OTP
        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(string[] gheDaChon, string maSuatChieu, string email)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
            {
                ViewBag.Error = "Bạn chưa chọn ghế!";
                LoadThongTinThanhToan(maSuatChieu, gheDaChon);
                return View("ThanhToan");
            }
                if (string.IsNullOrEmpty(email))
                {
                ViewBag.Error = "Bạn chưa nhập Email!";
                ViewBag.GheDaChon = gheDaChon;
                ViewBag.MaSuatChieu = maSuatChieu;
                ViewBag.Email = email;
                LoadThongTinThanhToan(maSuatChieu, gheDaChon);
                return View("ThanhToan");
                }
            

            // ✅ Sinh OTP và lưu Session
            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString(SessionOtpCode, otp);
            HttpContext.Session.SetString(SessionEmailTemp, email);
            HttpContext.Session.SetString(SessionGheDaChon, string.Join("|", gheDaChon));
            HttpContext.Session.SetString(SessionSuatChieu, maSuatChieu);

            try
            {
                await _emailService.SendEmailAsync(
                    email,
                    "🔐 Mã OTP xác nhận đặt vé xem phim",
                    $"Mã OTP của bạn là: {otp}"
                );
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi gửi email: " + ex.Message;
                ViewBag.GheDaChon = gheDaChon;
                ViewBag.MaSuatChieu = maSuatChieu;
                ViewBag.Email = email;

                LoadThongTinThanhToan(maSuatChieu, gheDaChon);
                return View("ThanhToan");
            }
            LoadThongTinThanhToan(maSuatChieu, gheDaChon);
            ViewBag.Message = "✅ Mã OTP đã gửi đến email của bạn.";
            TempData["Message"] = "✅ Mã OTP đã gửi đến email của bạn.";
            return View("NhapOTP");
        }
        [HttpGet]
        public IActionResult NhapOTP()
        {
            var maSuatChieu = HttpContext.Session.GetString(SessionSuatChieu);
            var gheDaChon = HttpContext.Session.GetString(SessionGheDaChon)?.Split('|') ?? new string[0];

            LoadThongTinThanhToan(maSuatChieu, gheDaChon);
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> XacNhanOTP(string otp)
        {
            var otpInSession = HttpContext.Session.GetString(SessionOtpCode);
            var gheDaChon = HttpContext.Session.GetString(SessionGheDaChon)?.Split('|') ?? new string[0];
            var maSuatChieu = HttpContext.Session.GetString(SessionSuatChieu);
            var suatChieu = _context.SuatChieu.FirstOrDefault(sc => sc.MaSuatChieu == maSuatChieu);
            var maNguoiDung = HttpContext.Session.GetString("MaNguoiDung");
            

            if (suatChieu == null)
            {
                ViewBag.Error = "Không tìm thấy suất chiếu!";
                return View("NhapOTP");
            }

            var maPhongChieu = suatChieu.MaPhongChieu;

            if (otp == otpInSession)
            {
                Ve? veDaiDien = null; // ✅ khai báo để lưu vé đầu tiên

                foreach (var ghe in gheDaChon)
                {
                    var gheTonTai = _context.Ghe.Any(g => g.TenGhe == ghe && g.MaPhongChieu == maPhongChieu);

                    if (!gheTonTai)
                    {
                        ViewBag.Error = $"❌ Ghế '{ghe}' không tồn tại trong phòng chiếu!";
                        LoadThongTinThanhToan(maSuatChieu!, gheDaChon);
                        return View("NhapOTP");
                    }

                    if (string.IsNullOrEmpty(maNguoiDung))
                    {
                        ViewBag.Error = "Không xác định được người dùng!";
                        LoadThongTinThanhToan(maSuatChieu!, gheDaChon);
                        return View("NhapOTP");
                    }

                    var veMoi = new Ve
                    {
                        MaVe = Guid.NewGuid().ToString(),
                        MaSuatChieu = maSuatChieu!,
                        TenGhe = ghe,
                        MaPhongChieu = maPhongChieu,
                        MaNguoiDung = maNguoiDung,
                        NgayDat = DateTime.Now,
                        TrangThai = "Đã thanh toán"
                    };

                    if (veDaiDien == null)
                        veDaiDien = veMoi; // ✅ chỉ gán 1 lần vé đầu tiên

                    _context.Ve.Add(veMoi);
                }
           
                await _context.SaveChangesAsync();
                var danhSachMaVe = string.Join("|", _context.Ve
               .Where(v => v.MaNguoiDung == maNguoiDung && v.MaSuatChieu == maSuatChieu && gheDaChon.Contains(v.TenGhe))
               .Select(v => v.MaVe)
               .ToList());
                HttpContext.Session.SetString("DanhSachMaVe", danhSachMaVe);
                HttpContext.Session.Remove(SessionOtpCode);
                return RedirectToAction("ThanhToanThanhCong", new { maVe = veDaiDien?.MaVe });
            }

            // ❌ Sai OTP → nạp lại thông tin để View không bị lỗi
            LoadThongTinThanhToan(maSuatChieu!, gheDaChon);
            ViewBag.Error = "❌ Mã OTP không đúng!";
            return View("NhapOTP");
        }

        [HttpPost]

        // ✅ Hàm load lại dữ liệu cho View
        private void LoadThongTinThanhToan(string maSuatChieu, string[] gheDaChon)
        {
            var suat = _context.SuatChieu
                                .Include(s => s.Phim)
                                .FirstOrDefault(s => s.MaSuatChieu == maSuatChieu);

            ViewBag.Suat = suat;
            ViewBag.GheDaChon = gheDaChon;
            ViewBag.TongTien = gheDaChon.Length * 45000;
        }

        // ✅ Hiển thị form thanh toán ban đầu
        [HttpPost]
        public IActionResult ThanhToanForm(string[] gheDaChon, string maSuatChieu)
        {
            if (gheDaChon == null || gheDaChon.Length == 0)
            {
                ViewBag.Error = "Bạn chưa chọn ghế!";
                return RedirectToAction("ChonGhe", new { maSuatChieu });
            }

            LoadThongTinThanhToan(maSuatChieu, gheDaChon);
            return View("ThanhToan");
        }


        // ✅ Trang thanh toán thành công
        public IActionResult ThanhToanThanhCong(string maVe)
        {
            var maVeListString = HttpContext.Session.GetString("DanhSachMaVe");

            if (string.IsNullOrEmpty(maVeListString))
                return RedirectToAction("Index", "Home");

            var maVeList = maVeListString.Split('|');

            var veList = _context.Ve
                .Include(v => v.SuatChieu)
                .ThenInclude(sc => sc.Phim)
                .Where(v => maVeList.Contains(v.MaVe))
                .ToList();

            if (veList == null || veList.Count == 0)
            {
                return NotFound("Không tìm thấy vé.");
            }

            // Nếu muốn truyền nhiều vé qua ViewBag
            ViewBag.DanhSachVe = veList;
            ViewBag.TongTien = veList.Count * 45000;
            var veDaiDien = veList.FirstOrDefault();
            if (veDaiDien != null && veDaiDien.SuatChieu?.ThoiGianChieu != null)
            {
                DateTime gioChieu = veDaiDien.SuatChieu.NgayChieu.Add(veDaiDien.SuatChieu.ThoiGianChieu);
                ViewBag.NgayChieu = gioChieu.ToString("dd/MM/yyyy");
                ViewBag.GioChieu = gioChieu.ToString("HH:mm");
            }

            return View();
        }

        public IActionResult ThongBaoThanhToanThanhCong()
        {
            return View();
        }
    }
}
