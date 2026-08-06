using MediatR;

namespace SMS.Application.Enrollments.Commands.EnrollStudent;

public record EnrollStudentCommand(Guid StudentId, Guid CourseId) : IRequest<Guid>;