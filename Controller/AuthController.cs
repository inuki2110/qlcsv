using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QLCSV.Data;   // Sửa namespace này theo project của bạn (VD: QLCSV.Models)
using QLCSV.Models; // Sửa namespace này theo project của bạn
using QLCSV.DTOs.Auth; // <--- Dòng này giúp tìm thấy RegisterRequest và LoginRequest

namespace QLCSV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 1. Đăng ký
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("Tài khoản đã tồn tại.");
            }

            // Tạo user mới (Ở đây lưu password thường, thực tế nên mã hóa bằng BCrypt)
            var user = new User
            {
                Username = request.Username,
                PasswordHash = request.Password, // Lưu ý: Database của bạn tên cột là Password hay PasswordHash? Sửa lại cho đúng nhé.
                FullName = request.FullName,
                Email = request.Email,
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!" });
        }

        // 2. Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Tìm user trong DB
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            // Kiểm tra pass (So sánh chuỗi thường)
            if (user == null || user.PasswordHash != request.Password)
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu.");
            }

            // Tạo Token JWT
            var token = CreateToken(user);
            return Ok(new { token = token, role = user.Role, username = user.Username });
        }

        // Hàm tạo Token
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}