using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class AuthController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        // Khai báo tên khóa session để lưu OTP và email
        private const string SessionOtpCode = "_OtpCode";
        private const string SessionEmailTemp = "_EmailTemp";

        public AuthController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // ====================== ĐĂNG KÝ ======================
        [HttpGet]
        public IActionResult DangKy() => View();

        [HttpPost]
        public IActionResult DangKy(DangKyView vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (_context.TaiKhoan.Any(t => t.TenDangNhap == vm.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "⚠️ Tên đăng nhập đã tồn tại.");
                return View(vm);
            }

            var nguoiDung = new NguoiDung
            {
                MaNguoiDung = Guid.NewGuid().ToString(),
                HoTen = vm.HoTen,
                Email = vm.Email,
                SDT = vm.SDT
            };

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = vm.TenDangNhap,
                MatKhau = vm.MatKhau,
                VaiTro = "KhachHang",
                IsLocked = false,
                MaNguoiDung = nguoiDung.MaNguoiDung,
                TrangThaiTK = "Đang hoạt động"
            };

            _context.NguoiDung.Add(nguoiDung);
            _context.TaiKhoan.Add(taiKhoan);
            _context.SaveChanges();

            TempData["ThongBao"] = "✅ Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        // ====================== ĐĂNG NHẬP ======================
        [HttpGet]
        public IActionResult DangNhap()
        {
            if (TempData["ThongBao"] != null)
                ViewBag.ThongBao = TempData["ThongBao"];
            return View();
        }

        [HttpPost]
        public IActionResult DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.ThongBao = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            // Đăng nhập admin tạm thời
            if (tenDangNhap == "admin" && matKhau == "1234")
            {
                HttpContext.Session.SetString("VaiTro", "Admin");
                HttpContext.Session.SetString("TenDangNhap", "admin");
                return RedirectToAction("Index", "Home");
            }

            var taiKhoan = _context.TaiKhoan.FirstOrDefault(x => x.TenDangNhap == tenDangNhap && x.MatKhau == matKhau);

            if (taiKhoan != null)
            {
                if (taiKhoan.TrangThaiTK.ToLower() == "đã khóa")
                {
                    ViewBag.ThongBao = "⚠️ Tài khoản của bạn đã bị khóa.";
                    return View();
                }

                HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);
                HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ThongBao = "Thông tin đăng nhập không đúng.";
            return View();
        }

        // ====================== ĐĂNG XUẤT ======================
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap");
        }

        // ====================== QUÊN MẬT KHẨU ======================

        // Bước 1: Nhập Email để gửi OTP
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View("~/Views/Auth/ForgotPassword.cshtml", model);

            var nguoiDung = _context.NguoiDung
                .Include(nd => nd.TaiKhoan)
                .FirstOrDefault(nd => nd.Email == model.Email);

            if (nguoiDung == null || nguoiDung.TaiKhoan == null)
            {
                ModelState.AddModelError("", "Email không tồn tại.");
                return View(model);
            }

            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString(SessionOtpCode, otp);
            HttpContext.Session.SetString(SessionEmailTemp, model.Email);

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("anikachross@gmail.com", "xtfm qdmd cvlg rwaf\r\n"),
                    EnableSsl = true
                };

                smtpClient.Send("anikachross@gmail.com", model.Email, "Mã OTP đặt lại mật khẩu", $"Mã OTP của bạn là: {otp}");

                TempData["Message"] = "✅ Mã OTP đã được gửi đến email của bạn.";
                return RedirectToAction("VerifyOTP");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi gửi email: " + ex.Message);
                return View(model);
            }
        }

        // Bước 2: Nhập OTP
        [HttpGet]
        public IActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOTP(string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                ViewBag.Error = "OTP không được để trống";
                return View();
            }

            var otpInSession = HttpContext.Session.GetString(SessionOtpCode);
            if (otp == otpInSession)
            {
                return RedirectToAction("ResetPassword");
            }

            ViewBag.Error = "❌ Mã OTP không đúng!";
            return View();
        }

        // Bước 3: Đặt lại mật khẩu
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var email = HttpContext.Session.GetString(SessionEmailTemp);
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Phiên làm việc hết hạn, vui lòng gửi lại OTP.";
                return View(model);
            }

            var user = _context.TaiKhoan
                .Include(t => t.NguoiDung)
                .FirstOrDefault(t => t.NguoiDung.Email == email);

            if (user == null)
            {
                ViewBag.Error = "❌ Không tìm thấy tài khoản!";
                return View(model);
            }

            user.MatKhau = model.NewPassword;
            _context.SaveChanges();

            // Xóa session sau khi đặt lại thành công
            HttpContext.Session.Remove(SessionEmailTemp);
            HttpContext.Session.Remove(SessionOtpCode);

            TempData["Message"] = "✅ Đặt lại mật khẩu thành công!";
            return RedirectToAction("DangNhap");
        }
    }
}


