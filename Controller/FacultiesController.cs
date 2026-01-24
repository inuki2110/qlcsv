using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCSV.Data;
using QLCSV.DTOs.Faculty;
using QLCSV.Models;

namespace QLCSV.Controllers
{
    [ApiController]
    [Route("api/faculties")]
    public class FacultyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacultyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/faculties
        [HttpGet]
        public async Task<ActionResult<List<FacultyResponse>>> GetAll()
        {
            var data = await _context.Faculties
                .Select(f => new FacultyResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    ShortName = f.ShortName
                })
                .ToListAsync();

            return Ok(data);
        }

        // POST: api/faculties
        [HttpPost]
        public async Task<IActionResult> Create(FacultyCreateRequest request)
        {
            var faculty = new Faculty
            {
                Name = request.Name,
                ShortName = request.ShortName
            };

            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/faculties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FacultyUpdateRequest request)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null) return NotFound();

            faculty.Name = request.Name;
            faculty.ShortName = request.ShortName;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/faculties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null) return NotFound();

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
