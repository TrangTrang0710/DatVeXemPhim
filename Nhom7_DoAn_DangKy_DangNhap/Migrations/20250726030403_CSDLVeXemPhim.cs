using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nhom7_DoAn_DangKy_DangNhap.Migrations
{
    /// <inheritdoc />
    public partial class CSDLVeXemPhim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "TaiKhoan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MaNguoiDung",
                table: "TaiKhoan",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SoLanDangNhapSai",
                table: "TaiKhoan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianKhoa",
                table: "TaiKhoan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoaiGhe",
                columns: table => new
                {
                    MaLoaiGhe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLoaiGhe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiGhe", x => x.MaLoaiGhe);
                });

            migrationBuilder.CreateTable(
                name: "LoaiPhong",
                columns: table => new
                {
                    MaLoaiPhong = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuongGhe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhong", x => x.MaLoaiPhong);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "Phim",
                columns: table => new
                {
                    MaPhim = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenPhim = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiLuong = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaoDien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayKhoiChieu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenDangNhap = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Anh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phim", x => x.MaPhim);
                    table.ForeignKey(
                        name: "FK_Phim_TaiKhoan_TenDangNhap",
                        column: x => x.TenDangNhap,
                        principalTable: "TaiKhoan",
                        principalColumn: "TenDangNhap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhongChieu",
                columns: table => new
                {
                    MaPhongChieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaLoaiPhong = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongChieu", x => x.MaPhongChieu);
                    table.ForeignKey(
                        name: "FK_PhongChieu_LoaiPhong_MaLoaiPhong",
                        column: x => x.MaLoaiPhong,
                        principalTable: "LoaiPhong",
                        principalColumn: "MaLoaiPhong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ghe",
                columns: table => new
                {
                    TenGhe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhongChieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLoaiGhe = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ghe", x => new { x.TenGhe, x.MaPhongChieu });
                    table.ForeignKey(
                        name: "FK_Ghe_LoaiGhe_MaLoaiGhe",
                        column: x => x.MaLoaiGhe,
                        principalTable: "LoaiGhe",
                        principalColumn: "MaLoaiGhe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ghe_PhongChieu_MaPhongChieu",
                        column: x => x.MaPhongChieu,
                        principalTable: "PhongChieu",
                        principalColumn: "MaPhongChieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuatChieu",
                columns: table => new
                {
                    MaSuatChieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhim = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhongChieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayChieu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianChieu = table.Column<TimeSpan>(type: "time", nullable: false),
                    GiaVe = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuatChieu", x => x.MaSuatChieu);
                    table.ForeignKey(
                        name: "FK_SuatChieu_Phim_MaPhim",
                        column: x => x.MaPhim,
                        principalTable: "Phim",
                        principalColumn: "MaPhim",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuatChieu_PhongChieu_MaPhongChieu",
                        column: x => x.MaPhongChieu,
                        principalTable: "PhongChieu",
                        principalColumn: "MaPhongChieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ve",
                columns: table => new
                {
                    MaVe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSuatChieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenGhe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhongChieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GheTenGhe = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GheMaPhongChieu = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ve", x => x.MaVe);
                    table.ForeignKey(
                        name: "FK_Ve_Ghe_GheTenGhe_GheMaPhongChieu",
                        columns: x => new { x.GheTenGhe, x.GheMaPhongChieu },
                        principalTable: "Ghe",
                        principalColumns: new[] { "TenGhe", "MaPhongChieu" });
                    table.ForeignKey(
                        name: "FK_Ve_Ghe_TenGhe_MaPhongChieu",
                        columns: x => new { x.TenGhe, x.MaPhongChieu },
                        principalTable: "Ghe",
                        principalColumns: new[] { "TenGhe", "MaPhongChieu" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ve_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ve_SuatChieu_MaSuatChieu",
                        column: x => x.MaSuatChieu,
                        principalTable: "SuatChieu",
                        principalColumn: "MaSuatChieu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    MaThanhToan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VeMaVe = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToan_Ve_MaVe",
                        column: x => x.MaVe,
                        principalTable: "Ve",
                        principalColumn: "MaVe",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThanhToan_Ve_VeMaVe",
                        column: x => x.VeMaVe,
                        principalTable: "Ve",
                        principalColumn: "MaVe");
                });

            migrationBuilder.InsertData(
                table: "LoaiGhe",
                columns: new[] { "MaLoaiGhe", "TenLoaiGhe" },
                values: new object[,]
                {
                    { "LG01", "Ghế thường" },
                    { "LG02", "Ghế VIP" },
                    { "LG03", "Ghế đôi" }
                });

            migrationBuilder.InsertData(
                table: "LoaiPhong",
                columns: new[] { "MaLoaiPhong", "SoLuongGhe" },
                values: new object[,]
                {
                    { "LP01", 80 },
                    { "LP02", 40 },
                    { "LP03", 40 }
                });

            migrationBuilder.InsertData(
                table: "NguoiDung",
                columns: new[] { "MaNguoiDung", "Email", "HoTen", "SDT" },
                values: new object[,]
                {
                    { "ND01", "huy246@gmail.com", "Nguyễn Gia Huy", "0901234567" },
                    { "ND02", "loantran@gmail.com", "Trần Thị Loan", "0912345678" },
                    { "ND03", "vanan@gmail.com", "Lê Văn An", "0923456789" },
                    { "ND04", "haiduong707@gmail.com", "Nguyễn An Hải Đường", "0934567890" },
                    { "ND05", "haichi0710@gmail.com", "Hoàng Phương Hải Chi ", "0923456366" },
                    { "ND06", "minhnguyen113@gmail.com", "Nguyễn Hoàng Nhật Minh ", "0324567890" }
                });

            migrationBuilder.InsertData(
                table: "Phim",
                columns: new[] { "MaPhim", "Anh", "DaoDien", "MoTa", "NgayKhoiChieu", "TenDangNhap", "TenPhim", "TheLoai", "ThoiLuong" },
                values: new object[,]
                {
                    { "P01", "dat-rung-phuong-nam.jpg ", "Nguyễn Quang Dũng", "Một cuộc phiêu lưu trong rừng U Minh Hạ", new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin2", "Đất Rừng Phương Nam", "Phiêu lưu", 120 },
                    { "P02", "bo-gia.jpg", "Trấn Thành ", "Câu chuyện về một người cha và cuộc sống Sài Gòn xưa", new DateTime(2025, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin1", "Bố già", "Hài", 100 },
                    { "P03", "chuyen-doi-bac-si-noi-tru.jpg", "Joo Dong-min ", "Kể về cuộc sống và tình bạn của các bác sĩ nội trú khoa sản tại bệnh viện Yulje, chi nhánh Jongno", new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin1", "Chuyện đời bác sĩ ngoại trú ", "Phim y khoa, tình cảm", 150 },
                    { "P04", "phim-han-quoc-ve-gioi-thuong-luu-kdrama-penthouse.jpg", "Joo Dong-min", "Cuộc chiến ngầm giữa các gia đình giàu có để giành địa vị và quyền lực, với những âm mưu, thủ đoạn và bi kịch đan xen", new DateTime(2022, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin2", "Cuộc chiến thượng lưu  ", "Chính kịch, tâm lý xã hội và gật gân", 120 },
                    { "P05", "lat-mat-6.jpg", "Lý Hải", "Cuộc đấu trí kịch tính và những cú lật bất ngờ", new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin2", "Lật mặt 6", "Hành động", 130 },
                    { "P06", "linh-mieu-quy-nhap-trang.jpg", "Lưu Thành Luân", "Xoay quanh câu chuyện về Linh Miêu, một loài mèo đen có khả năng nhìn thấy thế giới tâm linh và những hiện tượng siêu nhiên.", new DateTime(2024, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin2", "Linh Miêu: Quỷ Nhập Tràng", "Kinh dị", 120 }
                });

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "admin1",
                columns: new[] { "IsLocked", "MaNguoiDung", "SoLanDangNhapSai", "ThoiGianKhoa" },
                values: new object[] { false, "ND06", 0, null });

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "admin2",
                columns: new[] { "IsLocked", "MaNguoiDung", "SoLanDangNhapSai", "ThoiGianKhoa" },
                values: new object[] { false, "ND05", 0, null });

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "use1",
                columns: new[] { "IsLocked", "MaNguoiDung", "SoLanDangNhapSai", "ThoiGianKhoa" },
                values: new object[] { false, "ND01", 0, null });

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "use2",
                columns: new[] { "IsLocked", "MaNguoiDung", "SoLanDangNhapSai", "ThoiGianKhoa" },
                values: new object[] { false, "ND04", 0, null });

            migrationBuilder.InsertData(
                table: "PhongChieu",
                columns: new[] { "MaPhongChieu", "MaLoaiPhong", "TenPhong" },
                values: new object[,]
                {
                    { "PC1", "LP01", "Phòng 1 (Thường)" },
                    { "PC2", "LP02", "Phòng 2 (VIP)" },
                    { "PC3", "LP03", "Phòng 3 (Couple)" }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "TenDangNhap", "IsLocked", "MaNguoiDung", "MatKhau", "SoLanDangNhapSai", "ThoiGianKhoa", "TrangThaiTK", "VaiTro" },
                values: new object[,]
                {
                    { "an789", false, "ND03", "Pass@789", 0, null, "Hoạt động", "KhachHang" },
                    { "loantran456", false, "ND02", "Pass@456", 0, null, "Hoạt động", "KhachHang" }
                });

            migrationBuilder.InsertData(
                table: "SuatChieu",
                columns: new[] { "MaSuatChieu", "GiaVe", "MaPhim", "MaPhongChieu", "NgayChieu", "ThoiGianChieu" },
                values: new object[,]
                {
                    { "SC1", 100000m, "P01", "PC1", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC10", 140000m, "P05", "PC2", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC100", 140000m, "P05", "PC2", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC101", 200000m, "P01", "PC2", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC102", 150000m, "P02", "PC2", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC103", 100000m, "P03", "PC3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC104", 120000m, "P04", "PC3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC105", 160000m, "P05", "PC3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC106", 140000m, "P01", "PC3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC107", 200000m, "P02", "PC3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC108", 150000m, "P03", "PC3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC109", 100000m, "P04", "PC1", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC11", 200000m, "P01", "PC2", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC110", 120000m, "P05", "PC1", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC111", 160000m, "P01", "PC1", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC112", 140000m, "P02", "PC1", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC113", 200000m, "P03", "PC1", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC114", 150000m, "P04", "PC1", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC115", 100000m, "P05", "PC2", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC116", 120000m, "P01", "PC2", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC117", 160000m, "P02", "PC2", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC118", 140000m, "P03", "PC2", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC119", 200000m, "P04", "PC2", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC12", 150000m, "P02", "PC2", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC120", 150000m, "P05", "PC2", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC121", 100000m, "P01", "PC3", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC122", 120000m, "P02", "PC3", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC123", 160000m, "P03", "PC3", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC124", 140000m, "P04", "PC3", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC125", 200000m, "P05", "PC3", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC126", 150000m, "P01", "PC3", new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC13", 100000m, "P03", "PC3", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC14", 120000m, "P04", "PC3", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC15", 160000m, "P05", "PC3", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC16", 140000m, "P01", "PC3", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC17", 200000m, "P02", "PC3", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC18", 150000m, "P03", "PC3", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC19", 100000m, "P04", "PC1", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC2", 120000m, "P02", "PC1", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC20", 120000m, "P05", "PC1", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC21", 160000m, "P01", "PC1", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC22", 140000m, "P02", "PC1", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC23", 200000m, "P03", "PC1", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC24", 150000m, "P04", "PC1", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC25", 100000m, "P05", "PC2", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC26", 120000m, "P01", "PC2", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC27", 160000m, "P02", "PC2", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC28", 140000m, "P03", "PC2", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC29", 200000m, "P04", "PC2", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC3", 160000m, "P03", "PC1", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC30", 150000m, "P05", "PC2", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC31", 100000m, "P01", "PC3", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC32", 120000m, "P02", "PC3", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC33", 160000m, "P03", "PC3", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC34", 140000m, "P04", "PC3", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC35", 200000m, "P05", "PC3", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC36", 150000m, "P01", "PC3", new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC37", 100000m, "P02", "PC1", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC38", 120000m, "P03", "PC1", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC39", 160000m, "P04", "PC1", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC4", 140000m, "P04", "PC1", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC40", 140000m, "P05", "PC1", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC41", 200000m, "P01", "PC1", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC42", 150000m, "P02", "PC1", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC43", 100000m, "P03", "PC2", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC44", 120000m, "P04", "PC2", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC45", 160000m, "P05", "PC2", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC46", 140000m, "P01", "PC2", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC47", 200000m, "P02", "PC2", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC48", 150000m, "P03", "PC2", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC49", 100000m, "P04", "PC3", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC5", 200000m, "P05", "PC1", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC50", 120000m, "P05", "PC3", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC51", 160000m, "P01", "PC3", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC52", 140000m, "P02", "PC3", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC53", 200000m, "P03", "PC3", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC54", 150000m, "P04", "PC3", new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC55", 100000m, "P05", "PC1", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC56", 120000m, "P01", "PC1", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC57", 160000m, "P02", "PC1", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC58", 140000m, "P03", "PC1", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC59", 200000m, "P04", "PC1", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC6", 150000m, "P01", "PC1", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC60", 150000m, "P05", "PC1", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC61", 100000m, "P01", "PC2", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC62", 120000m, "P02", "PC2", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC63", 160000m, "P03", "PC2", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC64", 140000m, "P04", "PC2", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC65", 200000m, "P05", "PC2", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC66", 150000m, "P01", "PC2", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC67", 100000m, "P02", "PC3", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC68", 120000m, "P03", "PC3", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC69", 160000m, "P04", "PC3", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC7", 100000m, "P02", "PC2", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC70", 140000m, "P05", "PC3", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC71", 200000m, "P01", "PC3", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC72", 150000m, "P02", "PC3", new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC73", 100000m, "P03", "PC1", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC74", 120000m, "P04", "PC1", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC75", 160000m, "P05", "PC1", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC76", 140000m, "P01", "PC1", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC77", 200000m, "P02", "PC1", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC78", 150000m, "P03", "PC1", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC79", 100000m, "P04", "PC2", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC8", 120000m, "P03", "PC2", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC80", 120000m, "P05", "PC2", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC81", 160000m, "P01", "PC2", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC82", 140000m, "P02", "PC2", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC83", 200000m, "P03", "PC2", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC84", 150000m, "P04", "PC2", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC85", 100000m, "P05", "PC3", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC86", 120000m, "P01", "PC3", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC87", 160000m, "P02", "PC3", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC88", 140000m, "P03", "PC3", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC89", 200000m, "P04", "PC3", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC9", 160000m, "P04", "PC2", new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC90", 150000m, "P05", "PC3", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC91", 100000m, "P01", "PC1", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC92", 120000m, "P02", "PC1", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC93", 160000m, "P03", "PC1", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) },
                    { "SC94", 140000m, "P04", "PC1", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 19, 0, 0, 0) },
                    { "SC95", 200000m, "P05", "PC1", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 20, 30, 0, 0) },
                    { "SC96", 150000m, "P01", "PC1", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 22, 30, 0, 0) },
                    { "SC97", 100000m, "P02", "PC2", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 10, 0, 0, 0) },
                    { "SC98", 120000m, "P03", "PC2", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 13, 0, 0, 0) },
                    { "SC99", 160000m, "P04", "PC2", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 16, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_MaNguoiDung",
                table: "TaiKhoan",
                column: "MaNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ghe_MaLoaiGhe",
                table: "Ghe",
                column: "MaLoaiGhe");

            migrationBuilder.CreateIndex(
                name: "IX_Ghe_MaPhongChieu",
                table: "Ghe",
                column: "MaPhongChieu");

            migrationBuilder.CreateIndex(
                name: "IX_Phim_TenDangNhap",
                table: "Phim",
                column: "TenDangNhap");

            migrationBuilder.CreateIndex(
                name: "IX_PhongChieu_MaLoaiPhong",
                table: "PhongChieu",
                column: "MaLoaiPhong");

            migrationBuilder.CreateIndex(
                name: "IX_SuatChieu_MaPhim",
                table: "SuatChieu",
                column: "MaPhim");

            migrationBuilder.CreateIndex(
                name: "IX_SuatChieu_MaPhongChieu",
                table: "SuatChieu",
                column: "MaPhongChieu");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_MaVe",
                table: "ThanhToan",
                column: "MaVe");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_VeMaVe",
                table: "ThanhToan",
                column: "VeMaVe");

            migrationBuilder.CreateIndex(
                name: "IX_Ve_GheTenGhe_GheMaPhongChieu",
                table: "Ve",
                columns: new[] { "GheTenGhe", "GheMaPhongChieu" });

            migrationBuilder.CreateIndex(
                name: "IX_Ve_MaNguoiDung",
                table: "Ve",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Ve_MaSuatChieu",
                table: "Ve",
                column: "MaSuatChieu");

            migrationBuilder.CreateIndex(
                name: "IX_Ve_TenGhe_MaPhongChieu",
                table: "Ve",
                columns: new[] { "TenGhe", "MaPhongChieu" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_NguoiDung_MaNguoiDung",
                table: "TaiKhoan",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_NguoiDung_MaNguoiDung",
                table: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "Ve");

            migrationBuilder.DropTable(
                name: "Ghe");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "SuatChieu");

            migrationBuilder.DropTable(
                name: "LoaiGhe");

            migrationBuilder.DropTable(
                name: "Phim");

            migrationBuilder.DropTable(
                name: "PhongChieu");

            migrationBuilder.DropTable(
                name: "LoaiPhong");

            migrationBuilder.DropIndex(
                name: "IX_TaiKhoan_MaNguoiDung",
                table: "TaiKhoan");

            migrationBuilder.DeleteData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "an789");

            migrationBuilder.DeleteData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "loantran456");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "TaiKhoan");

            migrationBuilder.DropColumn(
                name: "MaNguoiDung",
                table: "TaiKhoan");

            migrationBuilder.DropColumn(
                name: "SoLanDangNhapSai",
                table: "TaiKhoan");

            migrationBuilder.DropColumn(
                name: "ThoiGianKhoa",
                table: "TaiKhoan");
        }
    }
}
