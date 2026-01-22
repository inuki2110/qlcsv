using QLCSV.Extensions;
using QLCSV.Middleware;

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

app.Run();
