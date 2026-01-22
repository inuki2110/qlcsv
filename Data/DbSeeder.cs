using Microsoft.EntityFrameworkCore;
using QLCSV.Models;

namespace QLCSV.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IConfiguration configuration)
        {
            // Kiểm tra xem đã có user nào chưa, nếu chưa thì tạo Admin
            if (!await context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    Email = "admin@qlcsv.com",
                    FullName = "Super Admin",
                    Role = "admin",
                    IsActive = true,
                    // Trong thực tế bạn cần mã hóa mật khẩu. Ở đây để demo mình gán cứng.
                    // Mật khẩu này sẽ dùng để đăng nhập: Admin@123
                    PasswordHash = "Admin@123",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}