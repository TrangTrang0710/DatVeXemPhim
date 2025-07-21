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
    
    public class SuatChieusController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public SuatChieusController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: SuatChieus
        public async Task<IActionResult> Index()
        {
            var nhom7_DoAn_DangKy_DangNhapContext = _context.SuatChieu.Include(s => s.Phim).Include(s => s.PhongChieu);
            var suatChieuList = _context.SuatChieu
           .Include(s => s.Phim)
           .Include(s => s.PhongChieu);
            return View(await suatChieuList.ToListAsync());
        }

        // GET: SuatChieus/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .FirstOrDefaultAsync(m => m.MaSuatChieu == id);
            if (suatChieu == null)
            {
                return NotFound();
            }

            return View(suatChieu);
        }

        // GET: SuatChieus/Create
        public IActionResult Create()
        {
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "MaPhim");
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "MaPhongChieu");
            return View();
        }

        // POST: SuatChieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSuatChieu,MaPhim,MaPhongChieu,ThoiGianChieu,GiaVe")] SuatChieu suatChieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suatChieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "MaPhim", suatChieu.MaPhim);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "MaPhongChieu", suatChieu.MaPhongChieu);
            return View(suatChieu);
        }

        // GET: SuatChieus/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suatChieu = await _context.SuatChieu.FindAsync(id);
            if (suatChieu == null)
            {
                return NotFound();
            }
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "MaPhim", suatChieu.MaPhim);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "MaPhongChieu", suatChieu.MaPhongChieu);
            return View(suatChieu);
        }

        // POST: SuatChieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSuatChieu,MaPhim,MaPhongChieu,ThoiGianChieu,GiaVe")] SuatChieu suatChieu)
        {
            if (id != suatChieu.MaSuatChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suatChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuatChieuExists(suatChieu.MaSuatChieu))
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
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "MaPhim", suatChieu.MaPhim);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "MaPhongChieu", suatChieu.MaPhongChieu);
            return View(suatChieu);
        }

        // GET: SuatChieus/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .FirstOrDefaultAsync(m => m.MaSuatChieu == id);
            if (suatChieu == null)
            {
                return NotFound();
            }

            return View(suatChieu);
        }

        // POST: SuatChieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var suatChieu = await _context.SuatChieu.FindAsync(id);
            if (suatChieu != null)
            {
                _context.SuatChieu.Remove(suatChieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuatChieuExists(string id)
        {
            return _context.SuatChieu.Any(e => e.MaSuatChieu == id);
        }
    }
}
