using System.Collections.Generic;

namespace QLCSV.Models
{
    public class Faculty
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? ShortName { get; set; }

        // Navigation
        public ICollection<Major> Majors { get; set; } = new List<Major>();
        public ICollection<AlumniProfile> AlumniProfiles { get; set; } = new List<AlumniProfile>();
    }
}
