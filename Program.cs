using Microsoft.EntityFrameworkCore;
using QLCSV.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerServices();

builder.WebHost.ConfigureWebServer();

var app = builder.Build();


app.UseSwaggerConfiguration(app.Environment);

if (!app.Environment.IsDevelopment())
{
    app.UseSecurityHeaders();
}

app.UseHttpsRedirection();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QLCSV.Data.AppDbContext>();
        context.Database.Migrate(); // Lệnh này sẽ tạo bảng nếu chưa có
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi tạo bảng Database.");
    }
}
app.Run();
