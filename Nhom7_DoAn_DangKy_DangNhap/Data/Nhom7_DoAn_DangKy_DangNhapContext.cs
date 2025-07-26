using Humanizer;
using Microsoft.EntityFrameworkCore;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nhom7_DoAn_DangKy_DangNhap.Data
{
    public class Nhom7_DoAn_DangKy_DangNhapContext : DbContext
    {
        public Nhom7_DoAn_DangKy_DangNhapContext (DbContextOptions<Nhom7_DoAn_DangKy_DangNhapContext> options)
            : base(options)
        {
        }
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.TaiKhoan> TaiKhoan { get; set; }

 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TaiKhoan → NguoiDung
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(tk => tk.NguoiDung)
                .WithOne(nd => nd.TaiKhoan)
                .HasForeignKey<TaiKhoan>(tk => tk.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình khóa chính tổng hợp cho Ghe
            modelBuilder.Entity<Ghe>()
                .HasKey(g => new { g.TenGhe, g.MaPhongChieu });

            modelBuilder.Entity<Ghe>()
                .HasOne(g => g.PhongChieu)
                .WithMany(p => p.Ghes)
                .HasForeignKey(g => g.MaPhongChieu);

            modelBuilder.Entity<Ghe>()
                .HasOne(g => g.LoaiGhe)
                .WithMany(l => l.Ghes)
                .HasForeignKey(g => g.MaLoaiGhe);

            // Ve → NguoiDung
            modelBuilder.Entity<Ve>()
                .HasOne(v => v.NguoiDung)
                .WithMany(n => n.Ves)
                .HasForeignKey(v => v.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict); // hoặc NoAction

            // Ve → SuatChieu
            modelBuilder.Entity<Ve>()
                .HasOne(v => v.SuatChieu)
                .WithMany(s => s.Ves)
                .HasForeignKey(v => v.MaSuatChieu)
                .OnDelete(DeleteBehavior.Restrict); // hoặc NoAction

            // Ve → Ghe (qua TenGhe + MaPhongChieu)
            modelBuilder.Entity<Ve>()
                .HasOne<Ghe>()
                .WithMany()
                .HasForeignKey(v => new { v.TenGhe, v.MaPhongChieu })
                .OnDelete(DeleteBehavior.Restrict); // hoặc NoAction
                                                    // ThanhToan → Ve
            modelBuilder.Entity<ThanhToan>()
                .HasOne<Ve>()
                .WithMany()
                .HasForeignKey(t => t.MaVe)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SuatChieu>()
                .Property(v => v.GiaVe)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<ThanhToan>()
                .Property(t => t.SoTien)
                .HasColumnType("decimal(18,2)");


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan { TenDangNhap = "admin1", MatKhau = "Admin@123", VaiTro = "Admin", TrangThaiTK = "Hoạt động" , MaNguoiDung = "ND06" },
                new TaiKhoan { TenDangNhap = "use1", MatKhau = "User@123", VaiTro = "KhachHang", TrangThaiTK = "Hoạt động" , MaNguoiDung = "ND01" },
                new TaiKhoan { TenDangNhap = "admin2", MatKhau = "Secure456", VaiTro = "Admin", TrangThaiTK = "Đã khóa" ,MaNguoiDung = "ND05" },
                new TaiKhoan { TenDangNhap = "use2", MatKhau = "User456", VaiTro = "KhachHang", TrangThaiTK = "Đã khóa" ,MaNguoiDung = "ND04"},
                new TaiKhoan { TenDangNhap = "loantran456", MatKhau = "Pass@456", VaiTro = "KhachHang", TrangThaiTK = "Hoạt động" ,MaNguoiDung = "ND02"},
                new TaiKhoan { TenDangNhap = "an789", MatKhau = "Pass@789", VaiTro = "KhachHang", TrangThaiTK = "Hoạt động" , MaNguoiDung = "ND03" }
                );
            modelBuilder.Entity<Phim>().HasData(
                new Phim { MaPhim = "P01", TenPhim = "Đất Rừng Phương Nam", TheLoai = "Phiêu lưu", ThoiLuong = 120, MoTa = "Một cuộc phiêu lưu trong rừng U Minh Hạ", DaoDien = "Nguyễn Quang Dũng", NgayKhoiChieu = new DateTime(2025, 07 ,20), TenDangNhap = "admin2",Anh = "dat-rung-phuong-nam.jpg ",TrailerUrl= "https://www.youtube.com/watch?v=hktzirCnJmQ" },
                new Phim { MaPhim = "P02", TenPhim = "Bố già", TheLoai = "Hài", ThoiLuong = 100, MoTa = "Câu chuyện về một người cha và cuộc sống Sài Gòn xưa", DaoDien = "Trấn Thành ", NgayKhoiChieu = new DateTime(2025, 10, 22), TenDangNhap = "admin1" , Anh ="bo-gia.jpg",TrailerUrl= "https://www.youtube.com/watch?v=jluSu8Rw6YE" },
                new Phim { MaPhim = "P03", TenPhim = "Chuyện đời bác sĩ ngoại trú ", TheLoai = "Phim y khoa, tình cảm", ThoiLuong = 150, MoTa = "Kể về cuộc sống và tình bạn của các bác sĩ nội trú khoa sản tại bệnh viện Yulje, chi nhánh Jongno", DaoDien = "Joo Dong-min ", NgayKhoiChieu = new DateTime(2025, 07, 22), TenDangNhap = "admin1" , Anh="chuyen-doi-bac-si-noi-tru.jpg",TrailerUrl= "https://www.youtube.com/watch?v=HZObM8GARJg" },
                new Phim { MaPhim = "P04", TenPhim = "Cuộc chiến thượng lưu  ", TheLoai = "Chính kịch, tâm lý xã hội và gật gân", ThoiLuong = 120, MoTa = "Cuộc chiến ngầm giữa các gia đình giàu có để giành địa vị và quyền lực, với những âm mưu, thủ đoạn và bi kịch đan xen", DaoDien = "Joo Dong-min", NgayKhoiChieu = new DateTime(2022, 06, 10), TenDangNhap = "admin2",Anh = "phim-han-quoc-ve-gioi-thuong-luu-kdrama-penthouse.jpg",TrailerUrl= "https://www.youtube.com/watch?v=NgD7nVVHAaQ" },
                new Phim { MaPhim = "P05", TenPhim = "Lật mặt 6", TheLoai = "Hành động", ThoiLuong = 130, MoTa = "Cuộc đấu trí kịch tính và những cú lật bất ngờ", DaoDien = "Lý Hải", NgayKhoiChieu = new DateTime(2022, 07, 25), TenDangNhap = "admin2", Anh = "lat-mat-6.jpg",TrailerUrl= "https://www.youtube.com/watch?v=o3FoowSoNr4" },
                new Phim { MaPhim = "P06", TenPhim = "Linh Miêu: Quỷ Nhập Tràng", TheLoai = "Kinh dị", ThoiLuong = 120, MoTa = "Xoay quanh câu chuyện về Linh Miêu, một loài mèo đen có khả năng nhìn thấy thế giới tâm linh và những hiện tượng siêu nhiên.", DaoDien = "Lưu Thành Luân", NgayKhoiChieu = new DateTime(2024, 11, 22), TenDangNhap = "admin2", Anh = "linh-mieu-quy-nhap-trang.jpg",TrailerUrl= "https://www.youtube.com/watch?v=XsPl7SbL2kg" }

            );
            modelBuilder.Entity<LoaiPhong>().HasData(
                new LoaiPhong { MaLoaiPhong = "Thuong", SoLuongGhe = 80 },
                new LoaiPhong { MaLoaiPhong = "VIP", SoLuongGhe = 40 },
                new LoaiPhong { MaLoaiPhong = "Couple", SoLuongGhe = 40 }
            );
            modelBuilder.Entity<PhongChieu>().HasData(
                new PhongChieu { MaPhongChieu = "PC01", TenPhong = "Phòng 1", MaLoaiPhong = "Thuong" },
                new PhongChieu { MaPhongChieu = "PC02", TenPhong = "Phòng 2", MaLoaiPhong = "VIP" },
                new PhongChieu { MaPhongChieu = "PC03", TenPhong = "Phòng 3", MaLoaiPhong = "Couple" }

            );

            modelBuilder.Entity<SuatChieu>().HasData(
                new SuatChieu { MaSuatChieu = "SC01", MaPhim = "P01", MaPhongChieu = "PC01", NgayChieu = DateTime.Today.AddDays(1), ThoiGianChieu = new TimeSpan(19, 0, 0), GiaVe = 120000.00m},
                new SuatChieu { MaSuatChieu = "SC02", MaPhim = "P02", MaPhongChieu = "PC01", NgayChieu = DateTime.Today.AddDays(2), ThoiGianChieu = new TimeSpan(21, 0, 0), GiaVe = 120000.00m },
                new SuatChieu { MaSuatChieu = "SC03", MaPhim = "P03", MaPhongChieu = "PC02", NgayChieu = DateTime.Today.AddDays(3), ThoiGianChieu = new TimeSpan(21, 0, 0), GiaVe = 150000.00m },
                new SuatChieu { MaSuatChieu = "SC04", MaPhim = "P04", MaPhongChieu = "PC03", NgayChieu = DateTime.Today.AddDays(5), ThoiGianChieu = new TimeSpan(20, 30, 0), GiaVe = 200000.00m },
                new SuatChieu { MaSuatChieu = "SC05", MaPhim = "P06", MaPhongChieu = "PC01", NgayChieu = DateTime.Today.AddDays(7), ThoiGianChieu = new TimeSpan(22, 30, 0), GiaVe = 120000.00m }
            );

            modelBuilder.Entity<LoaiGhe>().HasData(
                new LoaiGhe { MaLoaiGhe = "LG01", TenLoaiGhe = "Ghế thường"},
                new LoaiGhe { MaLoaiGhe = "LG02", TenLoaiGhe = "Ghế VIP"},
                new LoaiGhe { MaLoaiGhe = "LG03", TenLoaiGhe = "Ghế đôi"}
            );

            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung{MaNguoiDung = "ND01",HoTen = "Nguyễn Gia Huy",Email = "huy246@gmail.com",SDT = "0901234567"},
                new NguoiDung{MaNguoiDung = "ND02",HoTen = "Trần Thị Loan",Email = "loantran@gmail.com", SDT = "0912345678"},
                new NguoiDung{MaNguoiDung = "ND03",HoTen = "Lê Văn An",Email = "vanan@gmail.com",SDT = "0923456789"},
                new NguoiDung{MaNguoiDung = "ND04",HoTen = "Nguyễn An Hải Đường",Email = "haiduong707@gmail.com", SDT = "0934567890" },
                new NguoiDung{MaNguoiDung = "ND05",HoTen = "Hoàng Phương Hải Chi ", Email = "haichi0710@gmail.com", SDT = "0923456366" },
                new NguoiDung{MaNguoiDung = "ND06",HoTen = "Nguyễn Hoàng Nhật Minh ", Email = "minhnguyen113@gmail.com", SDT = "0324567890" }
            );


        }
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.Phim> Phim { get; set; } 
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.LoaiPhong> LoaiPhong { get; set; } 
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.PhongChieu> PhongChieu { get; set; } 
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.SuatChieu> SuatChieu { get; set; } 
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.LoaiGhe> LoaiGhe { get; set; }
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.Ghe> Ghe { get; set; } 
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.NguoiDung> NguoiDung { get; set; }
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.Ve> Ve { get; set; } 
        public DbSet<Nhom7_DoAn_DangKy_DangNhap.Models.ThanhToan> ThanhToan { get; set; } 
    }
}
