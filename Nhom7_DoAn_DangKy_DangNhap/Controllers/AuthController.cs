using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                TrangThaiTK = "Hoạt động",
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
        public async Task<IActionResult> DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.ThongBao = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            var taiKhoan = _context.TaiKhoan
                .AsNoTracking()
                .FirstOrDefault(x => x.TenDangNhap == tenDangNhap && x.MatKhau == matKhau);

            if (taiKhoan != null)
            {
                // ✅ Lưu Session (nếu cần)
                HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);
                HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);

                // ✅ Thêm Cookie Authentication
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
            new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaNguoiDung),
            new Claim(ClaimTypes.Role, taiKhoan.VaiTro)
        };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Thông tin đăng nhập không đúng.";
                return View();
            }
        
        }

        public async Task<IActionResult> DangXuat()
        {
            // Xoá session khi đăng xuất
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("Cookies");
            // Chuyển hướng về trang đăng nhập hoặc trang chủ
            TempData["ThongBao"] = "✅ Đã đăng xuất thành công.";
            return RedirectToAction("Index", "Home");
        }

    }
}
