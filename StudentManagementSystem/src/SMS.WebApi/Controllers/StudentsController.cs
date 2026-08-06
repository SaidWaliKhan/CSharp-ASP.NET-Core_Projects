using MediatR;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Students.Commands.CreateStudent;
using SMS.Application.Students.Commands.DeleteStudent;
using SMS.Application.Students.Commands.UpdateStudent;
using SMS.Application.Students.Queries.GetAllStudents;
using SMS.Application.Students.Queries.GetStudentById;

namespace SMS.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ISender _mediator;

    public StudentsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(
        [FromBody] CreateStudentCommand command,
        CancellationToken cancellationToken)
    {
        var studentId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetStudentById), new { id = studentId }, new { id = studentId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudentById(Guid id, CancellationToken cancellationToken)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id), cancellationToken);
        return Ok(student);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents(CancellationToken cancellationToken)
    {
        var students = await _mediator.Send(new GetAllStudentsQuery(), cancellationToken);
        return Ok(students);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent(
        Guid id,
        [FromBody] UpdateStudentCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route Id and body Id must match.");

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteStudentCommand(id), cancellationToken);
        return NoContent();
    }
}