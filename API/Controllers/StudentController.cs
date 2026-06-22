using Application.Features.Students.Create;
using Application.Features.Students.Queries.GetStudent;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{rollNo}")]
    public async Task<ActionResult<StudentResult>> GetStudent(string rollNo)
    {
        var student = await _mediator.Send(new GetStudentQuery(rollNo));

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<string>> AddStudent([FromBody] CreateStudentsCommand command)
    {
        var rollNo = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetStudent), new { rollNo }, rollNo);
    }
}
