using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Diagnostics;

namespace Nhom7_DoAn_DangKy_DangNhap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Nhom7_DoAn_DangKy_DangNhapContext _context;

        public HomeController(ILogger<HomeController> logger, Nhom7_DoAn_DangKy_DangNhapContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var dangChieu = _context.Phim
                .Where(p => p.NgayKhoiChieu <= DateTime.Today)
                .ToList();

            var sapChieu = _context.Phim
                .Where(p => p.NgayKhoiChieu > DateTime.Today)
                .ToList();

            var model = new TrangChuPhimViewModel
            {
                DangChieu = dangChieu ,
                SapChieu = sapChieu 
            };

            
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
