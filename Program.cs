using Microsoft.EntityFrameworkCore;
using QLCSV.Extensions; // Đảm bảo các file Extension này tồn tại
using QLCSV.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVICES (ĐĂNG KÝ DỊCH VỤ)
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Đăng ký các dịch vụ hệ thống (Database, JWT, Swagger...)
// Lưu ý: Trong hàm AddApplicationServices của bạn phải có cấu hình JWT (AddAuthentication)
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerServices();

builder.WebHost.ConfigureWebServer();

var app = builder.Build();

// ==========================================
// 2. MIDDLEWARE (LUỒNG XỬ LÝ) - QUAN TRỌNG
// ==========================================

// A. Swagger (Luôn bật để dễ test trên Render)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLCSV API V1");
    c.RoutePrefix = string.Empty; // Mở web lên là thấy Swagger ngay
});

// B. Bảo mật HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseSecurityHeaders();
}
app.UseHttpsRedirection();

// C. XÁC THỰC & PHÂN QUYỀN (MỚI THÊM)
// Hai dòng này BẮT BUỘC phải nằm trước MapControllers
app.UseAuthentication(); // 1. Kiểm tra Token: "Anh là ai?"
app.UseAuthorization();  // 2. Kiểm tra Quyền: "Anh được làm gì?"

// D. Định tuyến
app.MapControllers();

// ==========================================
// 3. AUTO MIGRATION & SEED DATA (TỰ ĐỘNG NẠP DỮ LIỆU)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var config = services.GetRequiredService<IConfiguration>();

        // Bước 1: Tạo Database & Bảng nếu chưa có
        Console.WriteLine("--> Đang kiểm tra Migration...");
        context.Database.Migrate();

        // Bước 2: Nạp dữ liệu mẫu (Gọi file DbSeeder.cs bạn vừa sửa)
        Console.WriteLine("--> Đang nạp dữ liệu mẫu...");
        await DbSeeder.SeedAsync(context, config);

        Console.WriteLine("--> HOÀN TẤT! Hệ thống đã sẵn sàng.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Lỗi nghiêm trọng khi khởi động Database.");
    }
}

app.Run();