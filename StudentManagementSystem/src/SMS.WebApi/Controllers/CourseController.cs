using MediatR;
using Microsoft.AspNetCore.Mvc;
using SMS.Application.Courses.Commands.CreateCourse;
using SMS.Application.Courses.Commands.DeleteCourse;
using SMS.Application.Courses.Commands.UpdateCourse;
using SMS.Application.Courses.Queries.GetAllCourses;
using SMS.Application.Courses.Queries.GetCourseById;

namespace SMS.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ISender _mediator;

    public CoursesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(
        [FromBody] CreateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var courseId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCourseById), new { id = courseId }, new { id = courseId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCourseById(Guid id, CancellationToken cancellationToken)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(id), cancellationToken);
        return Ok(course);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses(CancellationToken cancellationToken)
    {
        var courses = await _mediator.Send(new GetAllCoursesQuery(), cancellationToken);
        return Ok(courses);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCourse(
        Guid id,
        [FromBody] UpdateCourseCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route Id and body Id must match.");

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCourseCommand(id), cancellationToken);
        return NoContent();
    }
}