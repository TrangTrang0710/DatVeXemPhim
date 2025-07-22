using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using System.Reflection.Emit;
var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<Nhom7_DoAn_DangKy_DangNhapContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Nhom7_DoAn_DangKy_DangNhapContext")
        ?? throw new InvalidOperationException("Connection string 'Nhom7_DoAn_DangKy_DangNhapContext' not found.")));

// Authentication cookie
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/DangNhap";
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

// Session chỉ gọi 1 lần
builder.Services.AddSession();

var app = builder.Build();

// Migrate và seed dữ liệu
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Nhom7_DoAn_DangKy_DangNhapContext>();
    var services = scope.ServiceProvider;

    context.Database.Migrate();
    SeadData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Dùng Session trước Authentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
