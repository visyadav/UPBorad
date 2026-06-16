using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{rollNo}")]
    public async Task<ActionResult<StudentResult>> GetStudent(string rollNo)
    {
        var student = await _context.StudentResults
            .Include(s => s.Subjects)
            .FirstOrDefaultAsync(s => s.RollNumber == rollNo);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }
}
