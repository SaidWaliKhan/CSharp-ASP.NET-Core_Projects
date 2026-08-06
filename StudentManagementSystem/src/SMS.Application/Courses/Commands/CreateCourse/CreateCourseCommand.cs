using MediatR;

namespace SMS.Application.Courses.Commands.CreateCourse;

public record CreateCourseCommand(string Title, int Credits, int MaxCapacity) : IRequest<Guid>;