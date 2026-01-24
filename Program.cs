using Microsoft.EntityFrameworkCore;
using QLCSV.Extensions;
using QLCSV.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerServices();

builder.WebHost.ConfigureWebServer();

var app = builder.Build();

// ===== MIDDLEWARE =====
app.UseSwaggerConfiguration(app.Environment);

if (!app.Environment.IsDevelopment())
{
    app.UseSecurityHeaders();
}

app.UseHttpsRedirection();
app.MapControllers();

// ===== MIGRATE + SEED DATABASE =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var config = services.GetRequiredService<IConfiguration>();

        // Tạo DB + bảng
        context.Database.Migrate();

        // Seed dữ liệu mẫu
        await DbSeeder.SeedAsync(context, config);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Lỗi khi migrate / seed database.");
    }
}

app.Run();
