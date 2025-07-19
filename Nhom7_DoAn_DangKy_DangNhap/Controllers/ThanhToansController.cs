using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class ThanhToansController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public ThanhToansController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: ThanhToans
        public async Task<IActionResult> Index()
        {
            var context = _context.ThanhToan.Include(t => t.Ve);
            return View(await context.ToListAsync());
        }

        // GET: ThanhToans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var thanhToan = await _context.ThanhToan
                .Include(t => t.Ve)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);

            if (thanhToan == null) return NotFound();

            return View(thanhToan);
        }

        // GET: ThanhToans/Create
        public IActionResult Create()
        {
            ViewData["MaVe"] = new SelectList(_context.Ve, "MaVe", "MaVe");
            return View();
        }

        // POST: ThanhToans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThanhToan,MaVe,NgayThanhToan,SoTien,PhuongThuc")] ThanhToan thanhToan)
        {
            if (!ModelState.IsValid)
            {
                ViewData["MaVe"] = new SelectList(_context.Ve, "MaVe", "MaVe", thanhToan.MaVe);
                return View(thanhToan);
            }

            var ve = await _context.Ve.FindAsync(thanhToan.MaVe);
            if (ve == null)
            {
                ModelState.AddModelError("", "❌ Vé không tồn tại.");
                ViewData["MaVe"] = new SelectList(_context.Ve, "MaVe", "MaVe", thanhToan.MaVe);
                return View(thanhToan);
            }

            if (_context.ThanhToan.Any(t => t.MaVe == thanhToan.MaVe))
            {
                ModelState.AddModelError("", "⚠️ Vé này đã được thanh toán.");
                ViewData["MaVe"] = new SelectList(_context.Ve, "MaVe", "MaVe", thanhToan.MaVe);
                return View(thanhToan);
            }

            thanhToan.MaThanhToan = "TT" + (_context.ThanhToan.Count() + 1).ToString("D2");
            thanhToan.NgayThanhToan = DateTime.Now;
            thanhToan.SoTien = 120000;

            ve.TrangThai = "Đã thanh toán";
            _context.Update(ve);

            _context.Add(thanhToan);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ThanhToans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var thanhToan = await _context.ThanhToan.FindAsync(id);
            if (thanhToan == null) return NotFound();

            ViewData["MaVe"] = new SelectList(_context.Ve, "MaVe", "MaVe", thanhToan.MaVe);
            return View(thanhToan);
        }

        // POST: ThanhToans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaThanhToan,MaVe,NgayThanhToan,SoTien,PhuongThuc")] ThanhToan thanhToan)
        {
            if (id != thanhToan.MaThanhToan) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.MaThanhToan))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaVe"] = new SelectList(_context.Ve, "MaVe", "MaVe", thanhToan.MaVe);
            return View(thanhToan);
        }

        // GET: ThanhToans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var thanhToan = await _context.ThanhToan
                .Include(t => t.Ve)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);

            if (thanhToan == null) return NotFound();

            return View(thanhToan);
        }

        // POST: ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thanhToan = await _context.ThanhToan.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToan.Remove(thanhToan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhToanExists(string id)
        {
            return _context.ThanhToan.Any(e => e.MaThanhToan == id);
        }

        // ====== ✅ MOCK Thanh toán MOMO ======

        // Gọi khi bấm nút thanh toán
        public IActionResult ThanhToanOnline(string maVe)
        {
            var ve = _context.Ve.FirstOrDefault(v => v.MaVe == maVe);
            if (ve == null || ve.TrangThai == "Đã thanh toán")
            {
                return Content("⚠️ Vé không tồn tại hoặc đã thanh toán.");
            }

            var redirectUrl = $"https://sandbox.momo.vn/mock_payment?orderId={maVe}&amount=120000&returnUrl=https://localhost:5001/ThanhToans/Callback";
            return Redirect(redirectUrl);
        }

        // Callback từ MoMo giả lập
        public async Task<IActionResult> Callback(string orderId, int amount, string resultCode = "0")
        {
            if (resultCode == "0")
            {
                var ve = await _context.Ve.FirstOrDefaultAsync(v => v.MaVe == orderId);
                if (ve != null)
                {
                    ve.TrangThai = "Đã thanh toán";

                    var thanhToan = new ThanhToan
                    {
                        MaThanhToan = "TT" + (_context.ThanhToan.Count() + 1).ToString("D2"),
                        MaVe = ve.MaVe,
                        NgayThanhToan = DateTime.Now,
                        SoTien = amount,
                        PhuongThuc = "MoMo"
                        
                    };

                    _context.ThanhToan.Add(thanhToan);
                    _context.Update(ve);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Details", "Ve", new { id = orderId });
            }

            return Content("❌ Thanh toán thất bại hoặc bị hủy.");
        }
    }
}
