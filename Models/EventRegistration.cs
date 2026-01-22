namespace QLCSV.Models
{
    public class EventRegistration
    {
        public long Id { get; set; }

        public long EventId { get; set; }
        public Event Event { get; set; } = null!;

        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTimeOffset RegisteredAt { get; set; } = DateTimeOffset.UtcNow;

        public string Status { get; set; } = "registered";
    }
}
