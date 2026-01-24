using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCSV.Data;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetReport(string faculty = "Tất cả", string nienKhoa = "Tất cả", string status = "Tất cả")
    {
        // 1. Lấy truy vấn cơ bản từ bảng Alumni
        var query = _context.AlumniProfiles.AsQueryable();

        // 2. Lọc theo Khoa (nếu không phải "Tất cả")
        if (faculty != "Tất cả")
            query = query.Where(a => a.Major.Faculty.Name == faculty);

        // 3. Lọc theo Niên khóa
        if (nienKhoa != "Tất cả" && int.TryParse(nienKhoa, out int year))
            query = query.Where(a => a.GraduationYear == year);

        // 4. Lọc theo trạng thái việc làm
        if (status == "Có việc")
            query = query.Where(a => !string.IsNullOrEmpty(a.Company));
        else if (status == "Thất nghiệp")
            query = query.Where(a => string.IsNullOrEmpty(a.Company));

        // 5. Tính toán các con số thống kê
        var totalAlumni = await query.CountAsync();
        var employed = await query.CountAsync(a => !string.IsNullOrEmpty(a.Company));
        var totalEvents = await _context.Events.CountAsync();

        // 6. Chuẩn bị dữ liệu biểu đồ (Thống kê theo năm tốt nghiệp chẳng hạn)
        var chartData = await query
            .GroupBy(a => a.GraduationYear)
            .Select(g => new { Label = g.Key.ToString(), Value = g.Count() })
            .OrderBy(x => x.Label)
            .ToListAsync();

        return Ok(new
        {
            TotalAlumni = totalAlumni,
            Employed = employed,
            Unemployed = totalAlumni - employed,
            TotalEvents = totalEvents,
            ChartData = chartData
        });
    }
}