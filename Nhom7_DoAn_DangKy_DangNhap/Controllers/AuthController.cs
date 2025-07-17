using Microsoft.AspNetCore.Mvc;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class AuthController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public AuthController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View(new TaiKhoan());
        }

        [HttpPost]
        public IActionResult DangKy(TaiKhoan taiKhoan, string XacNhanMatKhau)
        {
            if (!ModelState.IsValid)
                return View(taiKhoan);

            if (taiKhoan.MatKhau != XacNhanMatKhau)
            {
                ViewBag.ThongBao = "❌ Mật khẩu xác nhận không khớp.";
                return View(taiKhoan);
            }

            if (_context.TaiKhoan.Any(t => t.TenDangNhap == taiKhoan.TenDangNhap))
            {
                ViewBag.ThongBao = "⚠️ Tên đăng nhập đã tồn tại.";
                return View(taiKhoan);
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(taiKhoan);
            }

            taiKhoan.VaiTro = "KhachHang";
            taiKhoan.TrangThaiTK = "Hoạt động";

            _context.TaiKhoan.Add(taiKhoan);
            _context.SaveChanges();

            TempData["ThongBao"] = "✅ Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");

        }

        [HttpGet]
        public IActionResult DangNhap()
        {
            if (TempData["ThongBao"] != null)
                ViewBag.ThongBao = TempData["ThongBao"];
            return View();
        }
    }
}
