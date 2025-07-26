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
    public class PhimsController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public PhimsController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: Phims
        public async Task<IActionResult> Index()
        {
            var nhom7_DoAn_DangKy_DangNhapContext = _context.Phim.Include(p => p.TaiKhoan);
            return View(await nhom7_DoAn_DangKy_DangNhapContext.ToListAsync());
        }

        // GET: Phims/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phim
                .Include(p => p.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // GET: Phims/Create
        public IActionResult Create()
        {
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoan, "TenDangNhap", "TenDangNhap");
            return View();
        }

        // POST: Phims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhim,TenPhim,TheLoai,ThoiLuong,MoTa,DaoDien,NgayKhoiChieu,TenDangNhap,Anh,TrailerUrl")] Phim phim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoan, "TenDangNhap", "TenDangNhap", phim.TenDangNhap);
            return View(phim);
        }

        // GET: Phims/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phim.FindAsync(id);
            if (phim == null)
            {
                return NotFound();
            }
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoan, "TenDangNhap", "TenDangNhap", phim.TenDangNhap);
            return View(phim);
        }

        // POST: Phims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaPhim,TenPhim,TheLoai,ThoiLuong,MoTa,DaoDien,NgayKhoiChieu,TenDangNhap,Anh,TrailerUrl")] Phim phim)
        {
            if (id != phim.MaPhim)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhimExists(phim.MaPhim))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoan, "TenDangNhap", "TenDangNhap", phim.TenDangNhap);
            return View(phim);
        }

        // GET: Phims/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phim
                .Include(p => p.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // POST: Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var phim = await _context.Phim.FindAsync(id);
            if (phim != null)
            {
                _context.Phim.Remove(phim);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhimExists(string id)
        {
            return _context.Phim.Any(e => e.MaPhim == id);
        }
    }
}
