using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nhom7_DoAn_DangKy_DangNhap.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TaiKhoan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC01",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC02",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 23, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC03",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 24, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC04",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC05",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "admin1",
                column: "Email",
                value: "");

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "admin2",
                column: "Email",
                value: "");

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "an789",
                column: "Email",
                value: "");

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "loantran456",
                column: "Email",
                value: "");

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "use1",
                column: "Email",
                value: "");

            migrationBuilder.UpdateData(
                table: "TaiKhoan",
                keyColumn: "TenDangNhap",
                keyValue: "use2",
                column: "Email",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "TaiKhoan");

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC01",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC02",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 21, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC03",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC04",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 24, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "SuatChieu",
                keyColumn: "MaSuatChieu",
                keyValue: "SC05",
                column: "NgayChieu",
                value: new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
