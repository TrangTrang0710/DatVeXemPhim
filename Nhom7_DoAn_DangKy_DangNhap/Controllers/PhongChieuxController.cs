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
    public class PhongChieuxController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public PhongChieuxController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: PhongChieux
        public async Task<IActionResult> Index()
        {
            var nhom7_DoAn_DangKy_DangNhapContext = _context.PhongChieu.Include(p => p.LoaiPhong);
            return View(await nhom7_DoAn_DangKy_DangNhapContext.ToListAsync());
        }

        // GET: PhongChieux/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongChieu = await _context.PhongChieu
                .Include(p => p.LoaiPhong)
                .FirstOrDefaultAsync(m => m.MaPhongChieu == id);
            if (phongChieu == null)
            {
                return NotFound();
            }

            return View(phongChieu);
        }

        // GET: PhongChieux/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiPhong"] = new SelectList(_context.LoaiPhong, "MaLoaiPhong", "MaLoaiPhong");
            return View();
        }

        // POST: PhongChieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhongChieu,TenPhong,MaLoaiPhong")] PhongChieu phongChieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phongChieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiPhong"] = new SelectList(_context.LoaiPhong, "MaLoaiPhong", "MaLoaiPhong", phongChieu.MaLoaiPhong);
            return View(phongChieu);
        }

        // GET: PhongChieux/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongChieu = await _context.PhongChieu.FindAsync(id);
            if (phongChieu == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiPhong"] = new SelectList(_context.LoaiPhong, "MaLoaiPhong", "MaLoaiPhong", phongChieu.MaLoaiPhong);
            return View(phongChieu);
        }

        // POST: PhongChieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaPhongChieu,TenPhong,MaLoaiPhong")] PhongChieu phongChieu)
        {
            if (id != phongChieu.MaPhongChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phongChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhongChieuExists(phongChieu.MaPhongChieu))
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
            ViewData["MaLoaiPhong"] = new SelectList(_context.LoaiPhong, "LoaiPhongId", "MaLoaiPhong", phongChieu.MaLoaiPhong);
            return View(phongChieu);
        }

        // GET: PhongChieux/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongChieu = await _context.PhongChieu
                .Include(p => p.LoaiPhong)
                .FirstOrDefaultAsync(m => m.MaPhongChieu == id);
            if (phongChieu == null)
            {
                return NotFound();
            }

            return View(phongChieu);
        }

        // POST: PhongChieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var phongChieu = await _context.PhongChieu.FindAsync(id);
            if (phongChieu != null)
            {
                _context.PhongChieu.Remove(phongChieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhongChieuExists(string id)
        {
            return _context.PhongChieu.Any(e => e.MaPhongChieu == id);
        }
    }
}
