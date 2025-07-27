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

        [HttpGet]
        public IActionResult ForgotPassword(string? tenDangNhap)
        {
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                TempData["Message"] = "⚠️ Bạn cần nhập tên đăng nhập trước khi yêu cầu quên mật khẩu.";
                return RedirectToAction("DangNhap");
            }

            var model = new ForgotPasswordViewModel
            {
                TenDangNhap = tenDangNhap
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var taiKhoan = _context.TaiKhoan
                .Include(t => t.NguoiDung)
                .FirstOrDefault(t =>
                    t.TenDangNhap == model.TenDangNhap &&
                    t.NguoiDung.Email == model.Email &&
                    t.TrangThaiTK != "Đã khóa");

            if (taiKhoan == null)
            {
                ModelState.AddModelError("", "❌ Email không khớp với tài khoản hoặc tài khoản không tồn tại.");
                return View(model);
            }

            var otp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString(SessionOtpCode, otp);
            HttpContext.Session.SetString(SessionEmailTemp, model.Email);
            HttpContext.Session.SetString("OtpCreatedTime", DateTime.Now.ToString("o"));
            HttpContext.Session.SetString("TenDangNhapTemp", model.TenDangNhap); // dùng lại nếu cần

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("anikachross@gmail.com", "xtfm qdmd cvlg rwaf"),
                    EnableSsl = true
                };

                var fromAddress = new MailAddress("anikachross@gmail.com", "Hệ thống đặt vé xem phim TX3");
                var toAddress = new MailAddress(model.Email);

                var subject = "🔐 Mã xác nhận đặt lại mật khẩu - Rạp phim TX3";

                var body = $@"
<div style='font-family: Arial, sans-serif; padding: 20px; color: #333; background-color: #f9f9f9; border-radius: 10px;'>
    <h2 style='color: #2c3e50;'>🎬 Rạp phim TX3</h2>
    <p>Xin chào <strong>{taiKhoan.NguoiDung.HoTen}</strong>,</p>
    <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản <strong>{taiKhoan.TenDangNhap}</strong>.</p>
    <p><strong>Mã OTP của bạn là:</strong></p>
    <div style='font-size: 28px; font-weight: bold; color: #e74c3c; padding: 10px 0;'>{otp}</div>
    <p>Vui lòng nhập mã này vào trang xác nhận để tiếp tục quá trình đặt lại mật khẩu.</p>
    <p style='color: gray; font-size: 13px;'>Lưu ý: Mã OTP có hiệu lực trong 1 phút kể từ khi gửi.</p>
    <hr style='margin: 20px 0;' />
    <p style='font-size: 12px; color: #999;'>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này hoặc liên hệ với bộ phận hỗ trợ của chúng tôi.</p>
    <p style='font-size: 13px;'>Trân trọng,<br><strong>Hệ thống đặt vé xem phim TX3</strong></p>
</div>";

                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                smtpClient.Send(message);

                TempData["Message"] = "✅ Mã OTP đã được gửi đến email của bạn.";
                return RedirectToAction("VerifyOTP");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi gửi email: " + ex.Message);
                return View(model);
            }
        }

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

            var createdTimeStr = HttpContext.Session.GetString("OtpCreatedTime");

            if (!DateTime.TryParse(createdTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var createdTime))
            {
                ViewBag.Error = "❌ Lỗi thời gian phiên OTP.";
                return View();
            }

            if ((DateTime.UtcNow - createdTime.ToUniversalTime()).TotalMinutes > 1)
            {
                ViewBag.Error = "❌ Mã OTP đã hết hạn. Vui lòng yêu cầu mã mới.";
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

            HttpContext.Session.Remove(SessionEmailTemp);
            HttpContext.Session.Remove(SessionOtpCode);

            TempData["Message"] = "✅ Đặt lại mật khẩu thành công!";
            return RedirectToAction("DangNhap");
        }

        [HttpPost]
        public IActionResult ResendOTP()
        {
            var email = HttpContext.Session.GetString(SessionEmailTemp);

            if (string.IsNullOrEmpty(email))
            {
                TempData["Message"] = "❗ Phiên làm việc đã hết. Vui lòng nhập lại email.";
                return RedirectToAction("ForgotPassword");
            }

            var taiKhoan = _context.TaiKhoan
                .Include(t => t.NguoiDung)
                .FirstOrDefault(t => t.NguoiDung.Email == email);

            if (taiKhoan == null)
            {
                TempData["Message"] = "❌ Không tìm thấy người dùng tương ứng.";
                return RedirectToAction("ForgotPassword");
            }

            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString(SessionOtpCode, otp);
            HttpContext.Session.SetString("OtpCreatedTime", DateTime.Now.ToString("o"));

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("anikachross@gmail.com", "xtfm qdmd cvlg rwaf"),
                    EnableSsl = true
                };

                var fromAddress = new MailAddress("anikachross@gmail.com", "Hệ thống đặt vé TX3");
                var toAddress = new MailAddress(email);

                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "🔄 Mã OTP mới - Rạp phim TX3",
                    Body = $"<p>Mã OTP mới của bạn là: <strong>{otp}</strong></p><p>Lưu ý: Mã có hiệu lực trong 1 phút.</p>",
                    IsBodyHtml = true
                };

                smtpClient.Send(message);

                TempData["Message"] = "✅ Mã OTP mới đã được gửi đến email.";
                return RedirectToAction("VerifyOTP");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "⚠️ Lỗi khi gửi lại mã: " + ex.Message;
                return RedirectToAction("ForgotPassword");
            }
        }
    }
}


