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
    public class NguoiDungsController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public NguoiDungsController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: NguoiDungs
        public async Task<IActionResult> Index()
        {
            var usersWithAccounts = _context.NguoiDung.Include(n => n.TaiKhoan);
            return View(await usersWithAccounts.ToListAsync());
        }

        // GET: NguoiDungs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var nguoiDung = await _context.NguoiDung
                .Include(n => n.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);

            if (nguoiDung == null) return NotFound();

            return View(nguoiDung);
        }

        // GET: NguoiDungs/Create
        public IActionResult Create()
        {
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoan, "TenDangNhap", "TenDangNhap");
            return View();
        }

        // POST: NguoiDungs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNguoiDung,HoTen,Email,SDT,TenDangNhap")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoan, "TenDangNhap", "TenDangNhap", nguoiDung.TaiKhoan?.TenDangNhap);
            return View(nguoiDung);
        }

        // GET: NguoiDungs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var nguoiDung = await _context.NguoiDung
                .Include(nd => nd.TaiKhoan)
                .FirstOrDefaultAsync(nd => nd.MaNguoiDung == id);

            if (nguoiDung == null) return NotFound();

            return View(nguoiDung);
        }

        // POST: NguoiDungs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNguoiDung,HoTen,Email,SDT")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingNguoiDung = await _context.NguoiDung
                        .Include(nd => nd.TaiKhoan)
                        .FirstOrDefaultAsync(nd => nd.MaNguoiDung == id);

                    if (existingNguoiDung == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật các thuộc tính
                    existingNguoiDung.HoTen = nguoiDung.HoTen;
                    existingNguoiDung.Email = nguoiDung.Email;
                    existingNguoiDung.SDT = nguoiDung.SDT;

                    // Không cập nhật TaiKhoan trực tiếp từ binding!
                    // Nếu bạn muốn cập nhật trạng thái, dùng checkbox/tùy chọn riêng và cập nhật tại đây

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.MaNguoiDung))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(nguoiDung);
        }


        // GET: NguoiDungs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var nguoiDung = await _context.NguoiDung
                .Include(n => n.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);

            if (nguoiDung == null) return NotFound();

            return View(nguoiDung);
        }

        // POST: NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nguoiDung = await _context.NguoiDung.FindAsync(id);
            if (nguoiDung != null)
            {
                _context.NguoiDung.Remove(nguoiDung);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(string id)
        {
            return _context.NguoiDung.Any(e => e.MaNguoiDung == id);
        }

       
    }
}
