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
    public class VesController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public VesController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: Ves
        public async Task<IActionResult> Index()
        {
            var nhom7_DoAn_DangKy_DangNhapContext = _context.Ve.Include(v => v.Ghe).Include(v => v.NguoiDung).Include(v => v.SuatChieu);
            return View(await nhom7_DoAn_DangKy_DangNhapContext.ToListAsync());
        }

        // GET: Ves/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ve = await _context.Ve
                .Include(v => v.Ghe)
                .Include(v => v.NguoiDung)
                .Include(v => v.SuatChieu)
                .FirstOrDefaultAsync(m => m.MaVe == id);
            if (ve == null)
            {
                return NotFound();
            }

            return View(ve);
        }

        // GET: Ves/Create
        public IActionResult Create()
        {
            ViewData["TenGhe"] = new SelectList(_context.Ghe, "TenGhe", "TenGhe");
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDung, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaSuatChieu"] = new SelectList(_context.SuatChieu, "MaSuatChieu", "MaSuatChieu");
            return View();
        }

        // POST: Ves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaVe,MaNguoiDung,MaSuatChieu,TenGhe,MaPhongChieu,NgayDat,TrangThai")] Ve ve)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ve);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenGhe"] = new SelectList(_context.Ghe, "TenGhe", "TenGhe", ve.TenGhe);
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDung, "MaNguoiDung", "MaNguoiDung", ve.MaNguoiDung);
            ViewData["MaSuatChieu"] = new SelectList(_context.SuatChieu, "MaSuatChieu", "MaSuatChieu", ve.MaSuatChieu);
            return View(ve);
        }

        // GET: Ves/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ve = await _context.Ve.FindAsync(id);
            if (ve == null)
            {
                return NotFound();
            }
            ViewData["TenGhe"] = new SelectList(_context.Ghe, "TenGhe", "TenGhe", ve.TenGhe);
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDung, "MaNguoiDung", "MaNguoiDung", ve.MaNguoiDung);
            ViewData["MaSuatChieu"] = new SelectList(_context.SuatChieu, "MaSuatChieu", "MaSuatChieu", ve.MaSuatChieu);
            return View(ve);
        }

        // POST: Ves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaVe,MaNguoiDung,MaSuatChieu,TenGhe,MaPhongChieu,NgayDat,TrangThai")] Ve ve)
        {
            if (id != ve.MaVe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ve);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeExists(ve.MaVe))
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
            ViewData["TenGhe"] = new SelectList(_context.Ghe, "TenGhe", "TenGhe", ve.TenGhe);
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDung, "MaNguoiDung", "MaNguoiDung", ve.MaNguoiDung);
            ViewData["MaSuatChieu"] = new SelectList(_context.SuatChieu, "MaSuatChieu", "MaSuatChieu", ve.MaSuatChieu);
            return View(ve);
        }

        // GET: Ves/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ve = await _context.Ve
                .Include(v => v.Ghe)
                .Include(v => v.NguoiDung)
                .Include(v => v.SuatChieu)
                .FirstOrDefaultAsync(m => m.MaVe == id);
            if (ve == null)
            {
                return NotFound();
            }

            return View(ve);
        }

        // POST: Ves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ve = await _context.Ve.FindAsync(id);
            if (ve != null)
            {
                _context.Ve.Remove(ve);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeExists(string id)
        {
            return _context.Ve.Any(e => e.MaVe == id);
        }

    }
}
