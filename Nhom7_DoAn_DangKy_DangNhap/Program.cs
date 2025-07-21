using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nhom7_DoAn_DangKy_DangNhap.Data;
using Nhom7_DoAn_DangKy_DangNhap.Models;
using Nhom7_DoAn_DangKy_DangNhap.Services;

var builder = WebApplication.CreateBuilder(args);

// Kết nối DB
builder.Services.AddDbContext<Nhom7_DoAn_DangKy_DangNhapContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Nhom7_DoAn_DangKy_DangNhapContext")
        ?? throw new InvalidOperationException("Connection string not found.")
    )
);

// Xác thực bằng Cookie
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/DangNhap"; // đường dẫn đăng nhập
    });

// Phân quyền
builder.Services.AddAuthorization();

// Session
builder.Services.AddSession();
// Đăng ký EmailService và cấu hình EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();


// Thêm MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Apply Migrations + Seed dữ liệu nếu cần
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Nhom7_DoAn_DangKy_DangNhapContext>();
    var services = scope.ServiceProvider;
    context.Database.Migrate(); // Auto migrate
    SeadData.Initialize(services); // Seed data ghế
}

// Xử lý exception nếu không phải chế độ development
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Session & Auth Middleware
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Định tuyến
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Chạy app
app.Run();
