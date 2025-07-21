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

        // GET: TaiKhoans
        public async Task<IActionResult> Index()
        {
            var danhSachTaiKhoan = await _context.TaiKhoan.ToListAsync();
            return View(danhSachTaiKhoan);
        }

        // GET: TaiKhoans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var taiKhoan = await _context.TaiKhoan
                .FirstOrDefaultAsync(m => m.TenDangNhap == id);

            if (taiKhoan == null)
                return NotFound();

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public IActionResult Create()
        {
           
            ViewBag.MaNguoiDung = new SelectList(_context.NguoiDung, "MaNguoiDung", "HoTen");
            return View();
            
        }

        // POST: TaiKhoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenDangNhap,MatKhau,VaiTro,TrangThaiTK,MaNguoiDung")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
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

            if (ModelState.IsValid)
            {
                try
                {
                    var taiKhoanCu = await _context.TaiKhoan.FindAsync(id);
                    if (taiKhoanCu == null)
                        return NotFound();

                    // Cập nhật các thuộc tính cho phép chỉnh sửa
                    taiKhoanCu.MatKhau = taiKhoan.MatKhau;
                    taiKhoanCu.VaiTro = taiKhoan.VaiTro;
                    taiKhoanCu.TrangThaiTK = taiKhoan.TrangThaiTK;
                    taiKhoanCu.MaNguoiDung = taiKhoan.MaNguoiDung;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.TenDangNhap))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var taiKhoan = await _context.TaiKhoan
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
    }
}
