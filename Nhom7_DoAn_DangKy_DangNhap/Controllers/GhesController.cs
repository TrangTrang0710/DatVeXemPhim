using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Threading.Tasks;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class GhesController : Controller
    {
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public GhesController(Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _context = context;
        }

        // GET: Ghes
        public async Task<IActionResult> Index()
        {
            var gheList = _context.Ghe.Include(g => g.LoaiGhe).Include(g => g.PhongChieu);
            return View(await gheList.ToListAsync());
        }

        // GET: Ghes/Details?tenGhe=A1&maPhongChieu=PC01
        public async Task<IActionResult> Details(string tenGhe, string maPhongChieu)
        {
            if (tenGhe == null || maPhongChieu == null)
                return NotFound();

            var ghe = await _context.Ghe
                .Include(g => g.LoaiGhe)
                .Include(g => g.PhongChieu)
                .FirstOrDefaultAsync(m => m.TenGhe == tenGhe && m.MaPhongChieu == maPhongChieu);

            if (ghe == null)
                return NotFound();

            return View(ghe);
        }

        // GET: Ghes/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiGhe"] = new SelectList(_context.LoaiGhe, "MaLoaiGhe", "TenLoaiGhe");
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "TenPhong");
            return View();
        }

        // POST: Ghes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenGhe,MaPhongChieu,MaLoaiGhe")] Ghe ghe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ghe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiGhe"] = new SelectList(_context.LoaiGhe, "MaLoaiGhe", "TenLoaiGhe", ghe.MaLoaiGhe);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "TenPhong", ghe.MaPhongChieu);
            return View(ghe);
        }

        // GET: Ghes/Edit?tenGhe=A1&maPhongChieu=PC01
        [HttpGet("Ghes/Edit/{tenGhe}/{maPhongChieu}")]
        public async Task<IActionResult> Edit(string tenGhe, string maPhongChieu)
        {
            if (tenGhe == null || maPhongChieu == null)
                return NotFound();

            var ghe = await _context.Ghe.FindAsync(tenGhe, maPhongChieu);
            if (ghe == null)
                return NotFound();

            ViewData["MaLoaiGhe"] = new SelectList(_context.LoaiGhe, "MaLoaiGhe", "MaLoaiGhe", ghe.MaLoaiGhe);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "MaPhongChieu", ghe.MaPhongChieu);
            return View(ghe);
        }

        [HttpPost("Ghes/Edit/{tenGhe}/{maPhongChieu}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string tenGhe, string maPhongChieu, [Bind("TenGhe,MaPhongChieu,MaLoaiGhe")] Ghe ghe)
        {
            if (tenGhe != ghe.TenGhe || maPhongChieu != ghe.MaPhongChieu)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ghe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Ghe.Any(e => e.TenGhe == tenGhe && e.MaPhongChieu == maPhongChieu))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiGhe"] = new SelectList(_context.LoaiGhe, "MaLoaiGhe", "TenLoaiGhe", ghe.MaLoaiGhe);
            ViewData["MaPhongChieu"] = new SelectList(_context.PhongChieu, "MaPhongChieu", "TenPhong", ghe.MaPhongChieu);
            return View(ghe);
        }


        // GET: Ghes/Delete?tenGhe=A1&maPhongChieu=PC01
        public async Task<IActionResult> Delete(string tenGhe, string maPhongChieu)
        {
            if (tenGhe == null || maPhongChieu == null)
                return NotFound();

            var ghe = await _context.Ghe
                .Include(g => g.LoaiGhe)
                .Include(g => g.PhongChieu)
                .FirstOrDefaultAsync(m => m.TenGhe == tenGhe && m.MaPhongChieu == maPhongChieu);

            if (ghe == null)
                return NotFound();

            return View(ghe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string tenGhe, string maPhongChieu)
        {
            var ghe = await _context.Ghe.FindAsync(new object[] { tenGhe, maPhongChieu });
            if (ghe != null)
            {
                _context.Ghe.Remove(ghe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GheExists(string tenGhe, string maPhongChieu)
        {
            return _context.Ghe.Any(e => e.TenGhe == tenGhe && e.MaPhongChieu == maPhongChieu);
        }
    }
}
