using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLCSV.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        // --- BỔ SUNG DÒNG NÀY ĐỂ SỬA LỖI CS0117 ---
        public string Username { get; set; } = null!;
        // ------------------------------------------

        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
        public string? EmailVerificationToken { get; set; }

        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = "pending";
        public bool IsActive { get; set; } = true;

        // Dùng DateTime để khớp với PostgreSQL và DbSeeder
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual AlumniProfile? AlumniProfile { get; set; }
        public virtual ICollection<Event> EventsCreated { get; set; } = new List<Event>();
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    }
}