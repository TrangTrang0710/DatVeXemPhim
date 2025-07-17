using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Models;

namespace Nhom7_DoAn_DangKy_DangNhap.Data
{
    public class Nhom7_DoAn_DangKy_DangNhapContext : DbContext
    {
        public Nhom7_DoAn_DangKy_DangNhapContext (DbContextOptions<Nhom7_DoAn_DangKy_DangNhapContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoan { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-4L6AN5E\SQLEXPRESS;Database=Nhom7_DoAn_DangKy_DangNhap;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan { TenDangNhap = "admin1", MatKhau = "Admin@123", VaiTro = "Admin", TrangThaiTK = "Hoạt động" },
                new TaiKhoan { TenDangNhap = "use1", MatKhau = "User@123", VaiTro = "KhachHang", TrangThaiTK = "Hoạt động"},
                new TaiKhoan { TenDangNhap = "admin2", MatKhau = "Secure456", VaiTro = "Admin", TrangThaiTK = "Đã khóa" },
                new TaiKhoan { TenDangNhap = "use2", MatKhau = "User456", VaiTro = "KhachHang", TrangThaiTK = "Đã khóa" }
                );
        }
    }
}
