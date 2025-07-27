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
            var nhom7_DoAn_DangKy_DangNhapContext = _context.NguoiDung.Include(n => n.TaiKhoan);
            return View(await nhom7_DoAn_DangKy_DangNhapContext.ToListAsync());
        }

        // GET: NguoiDungs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung
                .Include(n => n.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // GET: NguoiDungs/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: NguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,Email,SDT,TenDangNhap")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                var lastUser = await _context.NguoiDung
                    .OrderByDescending(u => u.MaNguoiDung)
                    .FirstOrDefaultAsync();

                string newId = "ND01";
                if (lastUser != null)
                {
                    int num = int.Parse(lastUser.MaNguoiDung.Substring(2));
                    newId = "ND" + (num + 1).ToString("D2");
                }

                nguoiDung.MaNguoiDung = newId;

                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nguoiDung);
        }

        // GET: NguoiDungs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.TaiKhoan, "MaNguoiDung", "TenDangNhap", nguoiDung.MaNguoiDung);
            return View(nguoiDung);
        }

        // POST: NguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNguoiDung,HoTen,Email,SDT,TenDangNhap")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.TaiKhoan, "MaNguoiDung", "TenDangNhap", nguoiDung.MaNguoiDung);
            return View(nguoiDung);
        }

        // GET: NguoiDungs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDung
                .Include(n => n.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(string id)
        {
            return _context.NguoiDung.Any(e => e.MaNguoiDung == id);
        }
        // Trang quản lý người dùng (chỉ Admin  hoặc nhân viên vào được)
        public async Task<IActionResult> QLNguoiDung()
        {
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            if (vaiTro != "Admin" && vaiTro != "NhanVien")
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang báo lỗi quyền
            }

            var dsNguoiDung = await _context.NguoiDung.Include(n => n.TaiKhoan).ToListAsync();
            return View(dsNguoiDung);
        }

        // KHÓA tài khoản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KhoaTaiKhoan(string id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var nguoiDung = await _context.NguoiDung
                .Include(nd => nd.TaiKhoan)
                .FirstOrDefaultAsync(nd => nd.MaNguoiDung == id);

            if (nguoiDung != null && nguoiDung.TaiKhoan != null)
            {
                nguoiDung.TaiKhoan.TrangThaiTK = "Khoa";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("QLNguoiDung");
        }

        // MỞ KHÓA tài khoản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoKhoaTaiKhoan(string id)
        {
            if (HttpContext.Session.GetString("VaiTro") != "Admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var nguoiDung = await _context.NguoiDung
                .Include(nd => nd.TaiKhoan)
                .FirstOrDefaultAsync(nd => nd.MaNguoiDung == id);

            if (nguoiDung != null && nguoiDung.TaiKhoan != null)
            {
                nguoiDung.TaiKhoan.TrangThaiTK = "Mo";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("QLNguoiDung");
        }
        [HttpPost]
        public async Task<IActionResult> LichSuVe()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (tenDangNhap == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var taiKhoan = await _context.TaiKhoan
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == tenDangNhap);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            var veDaDat = await _context.Ve
                .Include(v => v.Ghe)
                .Include(v => v.SuatChieu)
                .ThenInclude(sc => sc!.Phim)
                .Where(v => v.MaNguoiDung == taiKhoan.MaNguoiDung)
                .OrderByDescending(v => v.NgayDat)
                .ToListAsync();

            return View(veDaDat);
        }


    }
}
