using MediatR;

namespace SMS.Application.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand(Guid Id, string Title, int Credits, int MaxCapacity) : IRequest;