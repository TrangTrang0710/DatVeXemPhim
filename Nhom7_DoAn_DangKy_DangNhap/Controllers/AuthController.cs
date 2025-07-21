using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Security.Claims;

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
            return View();
        }

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

            // 1. Tạo người dùng
            var nguoiDung = new NguoiDung
            {
                MaNguoiDung = Guid.NewGuid().ToString(),
                HoTen = vm.HoTen,
                Email = vm.Email,
                SDT = vm.SDT
            };

            // 2. Tạo tài khoản
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = vm.TenDangNhap,
                MatKhau = vm.MatKhau,
                VaiTro = "KhachHang",
                IsLocked = false,
                MaNguoiDung = nguoiDung.MaNguoiDung
            };

            // 3. Lưu vào database
            _context.NguoiDung.Add(nguoiDung);
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

            [HttpPost]
            public IActionResult DangNhap(string tenDangNhap, string matKhau)
            {
                if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
                {
                    ViewBag.ThongBao = "Vui lòng điền đầy đủ thông tin.";
                    return View();
                }

                // Đăng nhập đặc biệt cho admin
                if (tenDangNhap == "admin" && matKhau == "1234")
                {
                    HttpContext.Session.SetString("VaiTro", "Admin");
                    HttpContext.Session.SetString("TenDangNhap", "admin");
                    return RedirectToAction("Index", "Home");
                }

                var taiKhoan = _context.TaiKhoan
                    .FirstOrDefault(x => x.TenDangNhap == tenDangNhap && x.MatKhau == matKhau);

                if (taiKhoan != null)
                {
                // ✅ Kiểm tra trạng thái tài khoản
                if (taiKhoan.TrangThaiTK.ToLower() == "đã khóa")
                {
                    ViewBag.ThongBao = "⚠️ Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.";
                        return View();
                    }

                    HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);
                    HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Thông tin đăng nhập không đúng.";
                    return View();
                }
            }
        public IActionResult DangXuat()
        {
            // Xoá session khi đăng xuất
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập hoặc trang chủ
            return RedirectToAction("DangNhap");
        }

    }
}
