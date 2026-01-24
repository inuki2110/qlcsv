using Microsoft.EntityFrameworkCore;
using QLCSV.Models;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq; // Cần thêm dòng này để dùng .Select, .ToList
using System.Threading.Tasks;

namespace QLCSV.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IConfiguration configuration)
        {
            // ====================================================
            // 1. TẠO KHOA (4 Khoa)
            // ====================================================
            if (!await context.Faculties.AnyAsync())
            {
                var faculties = new List<Faculty>
                {
                    new Faculty { Name = "Công nghệ thông tin", ShortName = "CNTT" },
                    new Faculty { Name = "Kinh tế", ShortName = "KT" },
                    new Faculty { Name = "Ngôn ngữ Anh", ShortName = "NNA" },
                    new Faculty { Name = "Cơ khí chế tạo", ShortName = "CK" }
                };
                context.Faculties.AddRange(faculties);
                await context.SaveChangesAsync();
            }

            // ====================================================
            // 2. TẠO NGÀNH (6 Ngành)
            // ====================================================
            if (!await context.Majors.AnyAsync())
            {
                // Lấy ID của các khoa vừa tạo
                var cntt = await context.Faculties.FirstAsync(f => f.ShortName == "CNTT");
                var kt = await context.Faculties.FirstAsync(f => f.ShortName == "KT");
                var nna = await context.Faculties.FirstAsync(f => f.ShortName == "NNA");
                var ck = await context.Faculties.FirstAsync(f => f.ShortName == "CK");

                var majors = new List<Major>
                {
                    new Major { Name = "Kỹ thuật phần mềm", Code = "SE", FacultyId = cntt.Id },
                    new Major { Name = "An toàn thông tin", Code = "IA", FacultyId = cntt.Id },
                    new Major { Name = "Quản trị kinh doanh", Code = "BA", FacultyId = kt.Id },
                    new Major { Name = "Marketing", Code = "MKT", FacultyId = kt.Id },
                    new Major { Name = "Biên phiên dịch", Code = "ENG", FacultyId = nna.Id },
                    new Major { Name = "Cơ điện tử", Code = "MEC", FacultyId = ck.Id }
                };
                context.Majors.AddRange(majors);
                await context.SaveChangesAsync();
            }

            // ====================================================
            // 3. TẠO TÀI KHOẢN ADMIN
            // ====================================================
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Role == "admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@qlcsv.com",
                    FullName = "Super Admin",
                    Role = "admin",
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }

            // ====================================================
            // 4. TẠO 30 SINH VIÊN MẪU (Sửa lỗi thiếu trường)
            // ====================================================
            if (!await context.Users.AnyAsync(u => u.Role == "user"))
            {
                var allMajors = await context.Majors.ToListAsync();
                var random = new Random();
                var usersToAdd = new List<User>();
                var profilesToAdd = new List<AlumniProfile>();

                string[] companies = { "FPT Software", "Viettel", "VNG", "VinGroup", "Shopee", "" };

                for (int i = 1; i <= 30; i++)
                {
                    // Tạo User
                    var user = new User
                    {
                        Username = $"sv{i}",
                        Email = $"sv{i}@gmail.com",
                        FullName = $"Cựu Sinh Viên {i}",
                        Role = "user",
                        IsActive = true,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Users.Add(user);
                    await context.SaveChangesAsync(); // Lưu để lấy ID

                    // Chọn ngẫu nhiên Ngành & Năm tốt nghiệp
                    var randomMajor = allMajors[random.Next(allMajors.Count)];
                    int gradYear = random.Next(2021, 2026);
                    string? company = companies[random.Next(companies.Length)];

                    // Tạo Hồ sơ (ĐÃ XÓA CÁC TRƯỜNG GÂY LỖI)
                    var profile = new AlumniProfile
                    {
                        UserId = user.Id,
                        FacultyId = randomMajor.FacultyId,
                        MajorId = randomMajor.Id,
                        // Đã xóa FullName, Phone, ClassClassName, City vì Model bạn không có
                        GraduationYear = gradYear,
                        Company = company,
                        CurrentPosition = string.IsNullOrEmpty(company) ? "Đang tìm việc" : "Nhân viên",
                        IsPublic = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    profilesToAdd.Add(profile);
                }

                context.AlumniProfiles.AddRange(profilesToAdd);
                await context.SaveChangesAsync();
            }

            // ====================================================
            // 5. TẠO SỰ KIỆN (Sửa lỗi thiếu Status)
            // ====================================================
            if (!await context.Events.AnyAsync())
            {
                // Vì Model Event của bạn không có Status, ta chỉ nhập Title và Date
                var events = new List<Event>
                {
                    new Event {
                        Title = "Họp mặt Cựu sinh viên 2026",
                        EventDate = DateTime.UtcNow.AddDays(30),
                        CreatedBy = adminUser.Id
                    },
                    new Event {
                        Title = "Hội thảo hướng nghiệp",
                        EventDate = DateTime.UtcNow.AddDays(15),
                        CreatedBy = adminUser.Id
                    },
                    new Event {
                        Title = "Lễ trao bằng tốt nghiệp",
                        EventDate = DateTime.UtcNow.AddDays(-10),
                        CreatedBy = adminUser.Id
                    }
                };
                context.Events.AddRange(events);
                await context.SaveChangesAsync();
            }
        }
    }
}