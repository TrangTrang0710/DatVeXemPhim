using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nhom7_DoAn_DangKy_DangNhap.Migrations
{
    /// <inheritdoc />
    public partial class DoAnDatVeXemPhim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiTK = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.TenDangNhap);
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "TenDangNhap", "MatKhau", "TrangThaiTK", "VaiTro" },
                values: new object[,]
                {
                    { "admin1", "Admin@123", "Hoạt động", "Admin" },
                    { "admin2", "Secure456", "Đã khóa", "Admin" },
                    { "use1", "User@123", "Hoạt động", "KhachHang" },
                    { "use2", "User456", "Đã khóa", "KhachHang" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
