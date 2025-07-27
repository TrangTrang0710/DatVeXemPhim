using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    
    public class TaiKhoansController : AuthController
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public TaiKhoansController(Nhom7_DoAn_DangKy_DangNhapContext context) : base(context)
        {
            _context = context;
        }

        // Hàm tạo mã người dùng theo vai trò: ADxxxx, NVxxxx, KHxxxx
        private string GenerateMaNguoiDungTheoVaiTro(string vaiTro)
        {
            string prefix = "KH"; // Mặc định là Khách hàng

            if (!string.IsNullOrEmpty(vaiTro))
            {
                switch (vaiTro.Trim().ToLower())
                {
                    case "admin":
                        prefix = "AD";
                        break;
                    case "nhân viên":
                    case "nhan vien":
                        prefix = "NV";
                        break;
                }
            }

            int count = 1;
            string maND;
            do
            {
                maND = prefix + count.ToString("D4");
                count++;
            } while (_context.NguoiDung.Any(nd => nd.MaNguoiDung == maND));

            return maND;
        }

        // GET: TaiKhoans
        public async Task<IActionResult> Index(string? sortOrder, string? vaiTro, string? trangThai, string? searchString)
        {
            var taiKhoans = _context.TaiKhoan.AsQueryable();

            // Tìm kiếm theo tên đăng nhập
            if (!string.IsNullOrEmpty(searchString))
            {
                taiKhoans = taiKhoans.Where(tk => tk.TenDangNhap.Contains(searchString));
            }

            // Lọc theo vai trò
            if (!string.IsNullOrEmpty(vaiTro))
            {
                taiKhoans = taiKhoans.Where(tk => tk.VaiTro == vaiTro);
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThai))
            {
                taiKhoans = taiKhoans.Where(tk => tk.TrangThaiTK == trangThai);
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "name_asc":
                    taiKhoans = taiKhoans.OrderBy(tk => tk.TenDangNhap);
                    break;
                case "name_desc":
                    taiKhoans = taiKhoans.OrderByDescending(tk => tk.TenDangNhap);
                    break;
            }

            return View(await taiKhoans.ToListAsync());
        }


        // GET: TaiKhoans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TenDangNhap,MatKhau,VaiTro,TrangThaiTK")] TaiKhoan taiKhoan,
            [Bind("HoTen,Email,SDT")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                // Tạo mã người dùng theo vai trò
                string maNguoiDung = GenerateMaNguoiDungTheoVaiTro(taiKhoan.VaiTro);
                nguoiDung.MaNguoiDung = maNguoiDung;

                taiKhoan.MaNguoiDung = maNguoiDung;

                _context.NguoiDung.Add(nguoiDung);
                _context.TaiKhoan.Add(taiKhoan);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var taiKhoan = await _context.TaiKhoan.FindAsync(id);
            if (taiKhoan == null)
                return NotFound();

            ViewBag.MaNguoiDung = new SelectList(_context.NguoiDung, "MaNguoiDung", "HoTen", taiKhoan.MaNguoiDung);
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TenDangNhap,MatKhau,VaiTro,TrangThaiTK,MaNguoiDung")] TaiKhoan taiKhoan)
        {
           
                if (id != taiKhoan.TenDangNhap)
                    return NotFound();

                var taiKhoanCu = await _context.TaiKhoan.FindAsync(id);
                if (taiKhoanCu == null)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    taiKhoanCu.VaiTro = taiKhoan.VaiTro;
                    taiKhoanCu.TrangThaiTK = taiKhoan.TrangThaiTK;
                    taiKhoanCu.MaNguoiDung = taiKhoan.MaNguoiDung;

                    // Chỉ cập nhật mật khẩu nếu người dùng nhập mới
                    if (!string.IsNullOrEmpty(taiKhoan.MatKhau))
                        taiKhoanCu.MatKhau = taiKhoan.MatKhau;

                    _context.Update(taiKhoanCu);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.MaNguoiDung = new SelectList(_context.NguoiDung, "MaNguoiDung", "HoTen", taiKhoan.MaNguoiDung);
                return View(taiKhoan);
            }


        // GET: TaiKhoans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var taiKhoan = await _context.TaiKhoan.Include(t => t.NguoiDung)
                .FirstOrDefaultAsync(m => m.TenDangNhap == id);

            if (taiKhoan == null)
                return NotFound();

            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taiKhoan = await _context.TaiKhoan.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoan.Remove(taiKhoan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(string id)
        {
            return _context.TaiKhoan.Any(e => e.TenDangNhap == id);
        }
        // GET: TaiKhoans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var taiKhoan = await _context.TaiKhoan
                .Include(t => t.NguoiDung)
                .FirstOrDefaultAsync(m => m.TenDangNhap == id);

            if (taiKhoan == null)
                return NotFound();

            return View(taiKhoan);
        }

    }

}
