using Microsoft.EntityFrameworkCore;
using QLCSV.Extensions;
using QLCSV.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Đăng ký các dịch vụ hệ thống
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerServices();

builder.WebHost.ConfigureWebServer();

var app = builder.Build();

// ===== MIDDLEWARE (ĐÃ SỬA ĐỂ CHẠY TRÊN RENDER) =====

// Luôn kích hoạt Swagger dù ở môi trường nào (Development hay Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLCSV API V1");
    // Đặt RoutePrefix là chuỗi trống để Swagger hiện ra ngay khi truy cập vào https://qlcsv-api.onrender.com/
    c.RoutePrefix = string.Empty;
});

// Các cấu hình bảo mật cho môi trường thực tế
if (!app.Environment.IsDevelopment())
{
    app.UseSecurityHeaders();
}

app.UseHttpsRedirection();
app.MapControllers();

// ===== THỰC THI MIGRATION + SEED DATABASE TỰ ĐỘNG =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var config = services.GetRequiredService<IConfiguration>();

        // Tự động tạo Database và các bảng nếu chưa có trên Cloud
        context.Database.Migrate();

        // Nạp dữ liệu mẫu ban đầu (Khoa, Ngành, Tài khoản mặc định)
        await DbSeeder.SeedAsync(context, config);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Lỗi khi thực hiện migrate hoặc nạp dữ liệu (seed) database.");
    }
}

app.Run();