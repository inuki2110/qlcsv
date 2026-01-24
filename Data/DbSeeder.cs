using Microsoft.EntityFrameworkCore;
using QLCSV.Models;
using BCrypt.Net;

namespace QLCSV.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IConfiguration configuration)
        {
            // ===============================
            // 1. FACULTY
            // ===============================
            if (!await context.Faculties.AnyAsync())
            {
                context.Faculties.AddRange(
                    new Faculty { Name = "Công nghệ thông tin", ShortName = "CNTT" },
                    new Faculty { Name = "Kinh tế", ShortName = "KTE" }
                );
                await context.SaveChangesAsync();
            }

            // ===============================
            // 2. MAJOR
            // ===============================
            if (!await context.Majors.AnyAsync())
            {
                var cntt = await context.Faculties.FirstAsync();

                context.Majors.Add(new Major
                {
                    Name = "Kỹ thuật phần mềm",
                    Code = "SE",
                    FacultyId = cntt.Id
                });

                await context.SaveChangesAsync();
            }

            // ===============================
            // 3. USERS (ADMIN + USER)
            // ===============================
            var admin = await context.Users.FirstOrDefaultAsync(u => u.Role == "admin");
            if (admin == null)
            {
                admin = new User
                {
                    Username = "admin",
                    Email = "admin@qlcsv.com",
                    FullName = "Super Admin",
                    Role = "admin",
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Role == "user");
            if (user == null)
            {
                user = new User
                {
                    Username = "user",
                    Email = "user@qlcsv.com",
                    FullName = "Nguyễn Văn A",
                    Role = "user",
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            // ===============================
            // 4. ALUMNI PROFILE
            // ===============================
            if (!await context.AlumniProfiles.AnyAsync())
            {
                var faculty = await context.Faculties.FirstAsync();
                var major = await context.Majors.FirstAsync();

                context.AlumniProfiles.Add(new AlumniProfile
                {
                    UserId = user.Id,
                    FacultyId = faculty.Id,
                    MajorId = major.Id,
                    GraduationYear = 2023,
                    CurrentPosition = "Backend Developer",
                    Company = "FPT Software",
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }

            // ===============================
            // 5. EVENT
            // ===============================
            if (!await context.Events.AnyAsync())
            {
                context.Events.Add(new Event
                {
                    Title = "Họp mặt Cựu Sinh Viên 2026",
                    EventDate = DateTime.UtcNow.AddDays(10),
                    CreatedBy = admin.Id
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
