using System.ComponentModel.DataAnnotations;

namespace QLCSV.DTOs.Event
{
    public class EventCreateRequest
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public string? Location { get; set; }

        public bool IsOnline { get; set; } = false;

        public string? MeetLink { get; set; }

        public string? ThumbnailUrl { get; set; }

        public int? MaxParticipants { get; set; }
    }
}
