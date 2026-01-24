namespace QLCSV.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int TotalAlumni { get; set; }
        public int Employed { get; set; }
        public int Unemployed { get; set; }

        public List<FacultyStatDto> FacultyStats { get; set; } = new List<FacultyStatDto>();
    }

    public class FacultyStatDto
    {
        public string FacultyName { get; set; } = "";
        public int Count { get; set; }
    }
}
