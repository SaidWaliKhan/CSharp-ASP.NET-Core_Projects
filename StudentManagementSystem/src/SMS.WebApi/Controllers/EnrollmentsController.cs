using MediatR;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Enrollments.Commands.AssignGrade;
using SMS.Application.Enrollments.Commands.EnrollStudent;
using SMS.Application.Enrollments.Queries.GetStudentEnrollments;

namespace SMS.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly ISender _mediator;

    public EnrollmentsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> EnrollStudent(
        [FromBody] EnrollStudentCommand command,
        CancellationToken cancellationToken)
    {
        var enrollmentId = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, new { id = enrollmentId });
    }

    [HttpPut("{id:guid}/grade")]
    public async Task<IActionResult> AssignGrade(
        Guid id,
        [FromBody] AssignGradeRequest body,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AssignGradeCommand(id, body.Grade), cancellationToken);
        return NoContent();
    }

    [HttpGet("students/{studentId:guid}")]
    public async Task<IActionResult> GetStudentEnrollments(Guid studentId, CancellationToken cancellationToken)
    {
        var enrollments = await _mediator.Send(new GetStudentEnrollmentsQuery(studentId), cancellationToken);
        return Ok(enrollments);
    }
}

public record AssignGradeRequest(string Grade);