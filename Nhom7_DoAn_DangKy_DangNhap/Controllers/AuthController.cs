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

            // Sinh mã NDxx tự động
            string newId = "ND01";
            var lastNguoiDung = _context.NguoiDung
                .OrderByDescending(nd => nd.MaNguoiDung)
                .FirstOrDefault();

            if (lastNguoiDung != null)
            {
                int num = int.Parse(lastNguoiDung.MaNguoiDung.Substring(2)) + 1;
                newId = "ND" + num.ToString("D2");
            }

            var nguoiDung = new NguoiDung
            {
                MaNguoiDung = newId,
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
                MaNguoiDung = newId,
                TrangThaiTK = "Đang hoạt động"
            };

            _context.NguoiDung.Add(nguoiDung);
            _context.TaiKhoan.Add(taiKhoan);
            _context.SaveChanges();

            TempData["ThongBao"] = "✅ Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }
        [HttpGet]
        public IActionResult DangNhap()
        {
            return View();
        }

        // ====================== ĐĂNG NHẬP ======================
        [HttpPost]
        public IActionResult DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.ThongBao = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            var taiKhoan = _context.TaiKhoan.FirstOrDefault(x => x.TenDangNhap == tenDangNhap);

            if (taiKhoan == null)
            {
                ViewBag.ThongBao = "Tên đăng nhập không tồn tại.";
                return View();
            }

            // ✅ Kiểm tra tài khoản bị khóa
            if (taiKhoan.IsLocked)
            {
                if (taiKhoan.ThoiGianKhoa.HasValue && DateTime.Now < taiKhoan.ThoiGianKhoa.Value.AddMinutes(15))
                {
                    var thoiGianConLai = taiKhoan.ThoiGianKhoa.Value.AddMinutes(15) - DateTime.Now;
                    ViewBag.ThongBao = $"⚠️ Tài khoản bị khóa. Vui lòng thử lại sau {thoiGianConLai.Minutes} phút {thoiGianConLai.Seconds} giây.";
                    return View();
                }
                else
                {
                    // ✅ Hết thời gian khóa → mở khóa
                    taiKhoan.IsLocked = false;
                    taiKhoan.SoLanDangNhapSai = 0;
                    taiKhoan.ThoiGianKhoa = null;
                }
            }

            // ✅ Kiểm tra mật khẩu
            if (taiKhoan.MatKhau == matKhau)
            {
                // Đăng nhập thành công → reset đếm sai
                taiKhoan.SoLanDangNhapSai = 0;
                _context.SaveChanges();

                HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);
                HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
                HttpContext.Session.SetString("MaNguoiDung", taiKhoan.MaNguoiDung);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Sai mật khẩu → tăng số lần sai
                taiKhoan.SoLanDangNhapSai += 1;

                if (taiKhoan.SoLanDangNhapSai >= 5)
                {
                    taiKhoan.IsLocked = true;

                    taiKhoan.ThoiGianKhoa = DateTime.Now;
                    _context.SaveChanges();

                    ViewBag.ThongBao = "⚠️ Tài khoản của bạn đã bị khóa 15 phút do nhập sai quá nhiều lần.";
                    return View();
                }

                _context.SaveChanges();
                ViewBag.ThongBao = $"Sai mật khẩu. Bạn còn {5 - taiKhoan.SoLanDangNhapSai} lần thử.";
                return View();
            }
        }


        // ====================== ĐĂNG XUẤT ======================
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap");
        }

        // ====================== QUÊN MẬT KHẨU ======================


        // Bước 1: Hiển thị form nhập email
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        // Bước 1: Gửi email OTP
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
                    Credentials = new NetworkCredential("anikachross@gmail.com", "xtfm qdmd cvlg rwaf"),
                    EnableSsl = true
                };

                var fromAddress = new MailAddress("anikachross@gmail.com", "Hệ thống đặt vé xem phim TX3");
                var toAddress = new MailAddress(model.Email);

                var subject = "🔐 Mã xác nhận đặt lại mật khẩu - Rạp phim TX3";

                var body = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px; color: #333; background-color: #f9f9f9; border-radius: 10px;'>
            <h2 style='color: #2c3e50;'>🎬 Rạp phim TX3</h2>
            <p>Xin chào,</p>
            <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
            <p><strong>Mã OTP của bạn là:</strong></p>
            <div style='font-size: 28px; font-weight: bold; color: #e74c3c; padding: 10px 0;'>{otp}</div>
            <p>Vui lòng nhập mã này vào trang xác nhận để tiếp tục quá trình đặt lại mật khẩu.</p>
            <p style='color: gray; font-size: 13px;'>Lưu ý: Mã OTP có hiệu lực trong 5 phút kể từ khi gửi.</p>
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
                .FirstOrDefault(t => t.NguoiDung!.Email == email);

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

    }
}


