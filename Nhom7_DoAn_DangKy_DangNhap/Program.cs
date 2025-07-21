using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Reflection.Emit;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Nhom7_DoAn_DangKy_DangNhapContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Nhom7_DoAn_DangKy_DangNhapContext") ?? throw new InvalidOperationException("Connection string 'Nhom7_DoAn_DangKy_DangNhapContext' not found.")));

// ✅ Thêm xác thực cookie
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/DangNhap"; // đường dẫn đến trang đăng nhập
    });

builder.Services.AddAuthorization(); // ✅ Thêm phần quyền

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// ✅ Tự động migrate DB và seed dữ liệu ghế
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Nhom7_DoAn_DangKy_DangNhapContext>();
    var services = scope.ServiceProvider;
   
    // Apply migrations (nếu chưa có)
    context.Database.Migrate();
    SeadData.Initialize(services);

}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication(); // ✅ Sử dụng xác thực cookie

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

builder.Services.AddSession(); 
app.UseSession();

app.Run();