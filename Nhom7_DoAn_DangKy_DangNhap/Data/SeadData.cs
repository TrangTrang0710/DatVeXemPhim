using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nhom7_DoAn_DangKy_DangNhap.Data
{
    public class SeadData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Nhom7_DoAn_DangKy_DangNhapContext(
                serviceProvider.GetRequiredService<DbContextOptions<Nhom7_DoAn_DangKy_DangNhapContext>>()))
            {
                context.Database.Migrate();

                // Kiểm tra bảng Ghế, nếu trống thì insert
                if (!context.Ghe.Any())
                {
                    var gheList = new List<Ghe>();

                    // Phòng Thường – PC1 – LG01
                    for (char row = 'A'; row <= 'H'; row++)
                        for (int seat = 1; seat <= 10; seat++)
                            gheList.Add(new Ghe { TenGhe = $"{row}{seat}", MaPhongChieu = "PC1", MaLoaiGhe = "LG01" });

                    // Phòng VIP – PC2 – LG02
                    for (char row = 'I'; row <= 'M'; row++)
                        for (int seat = 1; seat <= 8; seat++)
                            gheList.Add(new Ghe { TenGhe = $"{row}{seat}", MaPhongChieu = "PC2", MaLoaiGhe = "LG02" });

                    // Phòng Couple – PC3 – LG03
                    for (char row = 'N'; row <= 'R'; row++)
                        for (int seat = 1; seat <= 8; seat++)
                            gheList.Add(new Ghe { TenGhe = $"{row}{seat}", MaPhongChieu = "PC3", MaLoaiGhe = "LG03" });

                    context.Ghe.AddRange(gheList);
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                }
                if (!context.NguoiDung.Any())
                {
                    var nguoiDungList = new List<NguoiDung>
                 {
                     new NguoiDung{MaNguoiDung = "ND01",HoTen = "Nguyễn Gia Huy",Email = "huy246@gmail.com",SDT = "0901234567", Role = "KhachHang"},
                     new NguoiDung{MaNguoiDung = "ND02",HoTen = "Trần Thị Loan",Email = "loantran@gmail.com", SDT = "0912345678", Role = "KhachHang"},
                     new NguoiDung{MaNguoiDung = "ND03",HoTen = "Lê Văn An",Email = "vanan@gmail.com",SDT = "0923456789", Role = "KhachHang"},
                     new NguoiDung{MaNguoiDung = "ND04",HoTen = "Nguyễn An Hải Đường",Email = "haiduong707@gmail.com", SDT = "0934567890", Role = "KhachHang" },
                     new NguoiDung{MaNguoiDung = "ND05",HoTen = "Hoàng Phương Hải Chi", Email = "haichi0710@gmail.com", SDT = "0923456366", Role = "NhanVien" },
                     new NguoiDung{MaNguoiDung = "ND06",HoTen = "Nguyễn Hoàng Nhật Minh", Email = "minhnguyen113@gmail.com", SDT = "0324567890", Role = "NhanVien" }
                 };

                    context.NguoiDung.AddRange(nguoiDungList);
                    context.SaveChanges();

                    // Thêm tài khoản tương ứng cho 2 nhân viên
                    var taiKhoanList = new List<TaiKhoan>
                    {
                        new TaiKhoan { TenDangNhap = "nhanvien1", MatKhau = "123456", VaiTro = "NhanVien", TrangThaiTK = "Hoạt động", MaNguoiDung = "ND05" },
                        new TaiKhoan { TenDangNhap = "nhanvien2", MatKhau = "123456", VaiTro = "NhanVien", TrangThaiTK = "Hoạt động", MaNguoiDung = "ND06" }
                    };

                    context.TaiKhoan.AddRange(taiKhoanList);
                    context.SaveChanges();
                }


                if (!context.Ve.Any())
                {
                    var veList = new List<Ve>
                    {
                        new Ve { MaVe = "VE01", MaNguoiDung = "ND01", MaSuatChieu = "SC1", TenGhe = "A1", MaPhongChieu = "PC1", NgayDat = new DateTime(2025, 7, 10), TrangThai = "Đã thanh toán" },
                        new Ve { MaVe = "VE02", MaNguoiDung = "ND02", MaSuatChieu = "SC2", TenGhe = "D5", MaPhongChieu = "PC1", NgayDat = new DateTime(2025, 7, 11), TrangThai = "Đã thanh toán" },
                        new Ve { MaVe = "VE03", MaNguoiDung = "ND03", MaSuatChieu = "SC3", TenGhe = "I8", MaPhongChieu = "PC2", NgayDat = new DateTime(2025, 7, 12), TrangThai = "Chưa thanh toán" }
                    };
                    context.Ve.AddRange(veList);
                    context.SaveChanges();
                    context.ChangeTracker.Clear();
                    Console.WriteLine("✅ Đã seed dữ liệu vé.");
                }



                if (!context.ThanhToan.Any())
                {
                    var thanhToanList = new List<ThanhToan>
                    {
                        new ThanhToan { MaThanhToan = "TT01", MaVe = "VE01", NgayThanhToan = new DateTime(2025, 7, 10), SoTien = 120000m, PhuongThuc = "MoMo" },
                        new ThanhToan { MaThanhToan = "TT02", MaVe = "VE02", NgayThanhToan = new DateTime(2025, 7, 11), SoTien = 120000m, PhuongThuc = "ZaloPay" }
                    };
                    context.ThanhToan.AddRange(thanhToanList);
                    context.SaveChanges();
                    Console.WriteLine();
                }
            }

        }
    }
}
