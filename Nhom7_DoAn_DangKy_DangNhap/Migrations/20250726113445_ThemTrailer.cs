using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nhom7_DoAn_DangKy_DangNhap.Migrations
{
    /// <inheritdoc />
    public partial class ThemTrailer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P01",
                column: "TrailerUrl",
                value: "https://www.youtube.com/watch?v=hktzirCnJmQ");

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P02",
                column: "TrailerUrl",
                value: "https://www.youtube.com/watch?v=jluSu8Rw6YE");

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P03",
                column: "TrailerUrl",
                value: "https://www.youtube.com/watch?v=Wq9hNdROKlc&list=PLcrqK4yweNz4Mr18e86etKS0lqQAzonSf");

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P04",
                column: "TrailerUrl",
                value: "https://www.youtube.com/watch?v=NgD7nVVHAaQ");

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P05",
                column: "TrailerUrl",
                value: "https://www.youtube.com/watch?v=o3FoowSoNr4");

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P06",
                column: "TrailerUrl",
                value: "https://www.youtube.com/watch?v=XsPl7SbL2kg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P01",
                column: "TrailerUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P02",
                column: "TrailerUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P03",
                column: "TrailerUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P04",
                column: "TrailerUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P05",
                column: "TrailerUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Phim",
                keyColumn: "MaPhim",
                keyValue: "P06",
                column: "TrailerUrl",
                value: null);
        }
    }
}
