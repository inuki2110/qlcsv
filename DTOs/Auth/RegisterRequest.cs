namespace QLCSV.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;

        // Kiểm tra xem đã có dòng này chưa
        public string Email { get; set; } = null!;

        public string Role { get; set; } = "alumni";
    }
}