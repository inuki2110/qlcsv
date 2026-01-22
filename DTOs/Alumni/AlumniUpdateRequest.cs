using System.ComponentModel.DataAnnotations;

namespace QLCSV.DTOs.Alumni
{
    public class AlumniUpdateRequest
    {
        public string? CurrentPosition { get; set; }

        public string? Company { get; set; }

        public string? CompanyIndustry { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        [ValidUrl]
        public string? LinkedinUrl { get; set; }

        [ValidUrl]
        public string? FacebookUrl { get; set; }

        public string? Bio { get; set; }

        public bool? IsPublic { get; set; }
    }
}
