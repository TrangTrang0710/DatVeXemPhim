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
                new Phim { MaPhim = "P01", TenPhim = "Đất Rừng Phương Nam", TheLoai = "Phiêu lưu", ThoiLuong = 120, MoTa = "Một cuộc phiêu lưu trong rừng U Minh Hạ", DaoDien = "Nguyễn Quang Dũng", NgayKhoiChieu = new DateTime(2025, 07 ,20), TenDangNhap = "admin2",Anh = "dat-rung-phuong-nam.jpg " },
                new Phim { MaPhim = "P02", TenPhim = "Bố già", TheLoai = "Hài", ThoiLuong = 100, MoTa = "Câu chuyện về một người cha và cuộc sống Sài Gòn xưa", DaoDien = "Trấn Thành ", NgayKhoiChieu = new DateTime(2025, 10, 22), TenDangNhap = "admin1" , Anh ="bo-gia.jpg"},
                new Phim { MaPhim = "P03", TenPhim = "Chuyện đời bác sĩ ngoại trú ", TheLoai = "Phim y khoa, tình cảm", ThoiLuong = 150, MoTa = "Kể về cuộc sống và tình bạn của các bác sĩ nội trú khoa sản tại bệnh viện Yulje, chi nhánh Jongno", DaoDien = "Joo Dong-min ", NgayKhoiChieu = new DateTime(2025, 07, 22), TenDangNhap = "admin1" , Anh="chuyen-doi-bac-si-noi-tru.jpg"},
                new Phim { MaPhim = "P04", TenPhim = "Cuộc chiến thượng lưu  ", TheLoai = "Chính kịch, tâm lý xã hội và gật gân", ThoiLuong = 120, MoTa = "Cuộc chiến ngầm giữa các gia đình giàu có để giành địa vị và quyền lực, với những âm mưu, thủ đoạn và bi kịch đan xen", DaoDien = "Joo Dong-min", NgayKhoiChieu = new DateTime(2022, 06, 10), TenDangNhap = "admin2",Anh = "phim-han-quoc-ve-gioi-thuong-luu-kdrama-penthouse.jpg"},
                new Phim { MaPhim = "P05", TenPhim = "Lật mặt 6", TheLoai = "Hành động", ThoiLuong = 130, MoTa = "Cuộc đấu trí kịch tính và những cú lật bất ngờ", DaoDien = "Lý Hải", NgayKhoiChieu = new DateTime(2022, 07, 25), TenDangNhap = "admin2", Anh = "lat-mat-6.jpg"},
                new Phim { MaPhim = "P06", TenPhim = "Linh Miêu: Quỷ Nhập Tràng", TheLoai = "Kinh dị", ThoiLuong = 120, MoTa = "Xoay quanh câu chuyện về Linh Miêu, một loài mèo đen có khả năng nhìn thấy thế giới tâm linh và những hiện tượng siêu nhiên.", DaoDien = "Lưu Thành Luân", NgayKhoiChieu = new DateTime(2024, 11, 22), TenDangNhap = "admin2", Anh = "linh-mieu-quy-nhap-trang.jpg" }

            );
            modelBuilder.Entity<LoaiPhong>().HasData(
                new LoaiPhong { MaLoaiPhong = "LP01", SoLuongGhe = 80 },
                new LoaiPhong { MaLoaiPhong = "LP02", SoLuongGhe = 40 },
                new LoaiPhong { MaLoaiPhong = "LP03", SoLuongGhe = 40 }
            );
            modelBuilder.Entity<PhongChieu>().HasData(
                new PhongChieu { MaPhongChieu = "PC1", TenPhong = "Phòng 1 (Thường)", MaLoaiPhong = "LP01" },
                new PhongChieu { MaPhongChieu = "PC2", TenPhong = "Phòng 2 (VIP)", MaLoaiPhong = "LP02" },
                new PhongChieu { MaPhongChieu = "PC3", TenPhong = "Phòng 3 (Couple)", MaLoaiPhong = "LP03" }
            );

            var suatChieuList = new List<SuatChieu>();
            int counter = 1;
            string[] phongList = { "PC1", "PC2", "PC3" };
            string[] phimList = { "P01", "P02", "P03", "P04", "P05" };
            TimeSpan[] khungGio = {
                new TimeSpan(10, 0, 0),
                new TimeSpan(13, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(19, 0, 0),
                new TimeSpan(20, 30, 0),
                new TimeSpan(22, 30, 0)
            };

            decimal[] giaVe = {
                 100000m, // 10:00
                 120000m, // 13:00
                 160000m, // 16:00
                 140000m, // 19:00
                 200000m, // 20:30
                 150000m  // 22:30
            };
            for (int day = 1; day <= 7; day++) // 7 ngày tới
            {
                foreach (var phong in phongList)
                {
                    for (int i = 0; i < khungGio.Length; i++)
                    {
                        var maSuat = "SC" + counter.ToString("D1");
                        var maPhim = phimList[(counter - 1) % phimList.Length];

                        suatChieuList.Add(new SuatChieu
                        {
                            MaSuatChieu = maSuat,
                            MaPhim = maPhim,
                            MaPhongChieu = phong,
                            NgayChieu = DateTime.Today.AddDays(day),
                            ThoiGianChieu = khungGio[i],
                            GiaVe = giaVe[i]
                        });

                        counter++;
                    }
                }
            }

            modelBuilder.Entity<SuatChieu>().HasData(suatChieuList);

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
