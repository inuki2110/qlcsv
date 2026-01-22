using System.ComponentModel.DataAnnotations;

namespace QLCSV.DTOs.Batch
{
    public class BatchUpdateRequest
    {
        public int GraduationYear { get; set; }

        public string Name { get; set; } = null!;

        public int? StartYear { get; set; }

        public string? Description { get; set; }
    }
}
