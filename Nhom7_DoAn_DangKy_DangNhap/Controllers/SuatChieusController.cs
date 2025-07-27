using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using X.PagedList;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class SuatChieusController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public SuatChieusController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // ✅ Index với bộ lọc + phân trang
        public IActionResult Index(string searchString, DateTime? dateFilter, string roomFilter, int page = 1)
        {
            int pageSize = 5;

            var query = _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .OrderBy(s => s.NgayChieu)
                .ThenBy(s => s.ThoiGianChieu)
                .AsQueryable();

            // Lọc theo tên phim
            if (!string.IsNullOrEmpty(searchString))
            {
                string keyword = searchString.ToLower();
                query = query.Where(s => s.Phim!.TenPhim.ToLower().Contains(keyword));
            }

            // Lọc theo ngày chiếu
            if (dateFilter.HasValue)
            {
                query = query.Where(s => s.NgayChieu.Date == dateFilter.Value.Date);
            }

            // Lọc theo phòng chiếu (MaPhongChieu)
            if (!string.IsNullOrEmpty(roomFilter))
            {
                query = query.Where(s => s.MaPhongChieu == roomFilter);
            }

            // Lấy danh sách phòng cho dropdown
            ViewBag.RoomList = _context.PhongChieu
                .Select(p => new { p.MaPhongChieu, p.TenPhong })
                .ToList();

            // Giữ giá trị lọc
            ViewBag.SearchString = searchString;
            ViewBag.DateFilter = dateFilter?.ToString("yyyy-MM-dd");
            ViewBag.RoomFilter = roomFilter;

            // Phân trang
            var pagedList = query.ToPagedList(page, pageSize);
            return View(pagedList);
        }

        // ✅ Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .FirstOrDefaultAsync(m => m.MaSuatChieu == id);

            if (suatChieu == null) return NotFound();

            return View(suatChieu);
        }

        // ✅ Create
        public IActionResult Create()
        {
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim");
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "TenPhong");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSuatChieu,MaPhim,MaPhongChieu,NgayChieu,ThoiGianChieu,GiaVe")] SuatChieu suatChieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suatChieu);
                await _context.SaveChangesAsync();
                TempData["Message"] = "✅ Thêm suất chiếu thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suatChieu.MaPhim);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "TenPhong", suatChieu.MaPhongChieu);
            return View(suatChieu);
        }

        // ✅ Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var suatChieu = await _context.SuatChieu.FindAsync(id);
            if (suatChieu == null) return NotFound();

            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suatChieu.MaPhim);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "TenPhong", suatChieu.MaPhongChieu);
            return View(suatChieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSuatChieu,MaPhim,MaPhongChieu,NgayChieu,ThoiGianChieu,GiaVe")] SuatChieu suatChieu)
        {
            if (id != suatChieu.MaSuatChieu) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suatChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SuatChieu.Any(e => e.MaSuatChieu == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(suatChieu);
        }

        // ✅ Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.PhongChieu)
                .FirstOrDefaultAsync(m => m.MaSuatChieu == id);

            if (suatChieu == null) return NotFound();

            return View(suatChieu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var suatChieu = await _context.SuatChieu.FindAsync(id);
            if (suatChieu != null)
            {
                _context.SuatChieu.Remove(suatChieu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
