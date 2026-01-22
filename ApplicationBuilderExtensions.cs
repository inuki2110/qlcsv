
using Microsoft.EntityFrameworkCore;
using QLCSV.Data;
using QLCSV.Middleware;

namespace QLCSV.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> UseDatabaseMigrationAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;


                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var configuration = services.GetRequiredService<IConfiguration>();


                    await context.Database.MigrateAsync();
                    Console.WriteLine("✅ Database migrations applied successfully!");

                    await DbSeeder.SeedAsync(context, configuration);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"⚠️  Database Error: {ex.Message}");
                    Console.WriteLine("⚠️  Application will continue. Database operations may fail if schema is incompatible.");
                }
            }

            return app;
        }

        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
                await next();
            });
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}
