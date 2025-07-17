using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nhom7_DoAn_DangKy_DangNhap.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Nhom7_DoAn_DangKy_DangNhapContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Nhom7_DoAn_DangKy_DangNhapContext") ?? throw new InvalidOperationException("Connection string 'Nhom7_DoAn_DangKy_DangNhapContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
