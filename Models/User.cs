namespace QLCSV.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;

        // --- BỔ SUNG 2 DÒNG NÀY ---
        public string PasswordHash { get; set; } = null!;
        public string? EmailVerificationToken { get; set; }
        // --------------------------

        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = "pending";
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public AlumniProfile? AlumniProfile { get; set; }
        public ICollection<Event> EventsCreated { get; set; } = new List<Event>();
        public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    }
}