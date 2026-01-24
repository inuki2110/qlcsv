using Microsoft.AspNetCore.Mvc;
using QLCSV.Data;
using QLCSV.DTOs.Dashboard;

namespace QLCSV.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            var total = _context.AlumniProfiles.Count();

            var employed = _context.AlumniProfiles
                .Count(a => !string.IsNullOrEmpty(a.Company));

            var unemployed = total - employed;

            var facultyStats = _context.AlumniProfiles
                .GroupBy(a => a.Faculty.Name)
                .Select(g => new FacultyStatDto
                {
                    FacultyName = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Ok(new DashboardDto
            {
                TotalAlumni = total,
                Employed = employed,
                Unemployed = unemployed,
                FacultyStats = facultyStats
            });
        }
    }
}
