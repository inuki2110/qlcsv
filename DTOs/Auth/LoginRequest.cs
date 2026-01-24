using System.ComponentModel.DataAnnotations;

namespace QLCSV.DTOs.Auth // <-- Bắt buộc phải có dòng này
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}