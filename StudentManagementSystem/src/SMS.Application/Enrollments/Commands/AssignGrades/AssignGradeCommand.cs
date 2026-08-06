using MediatR;

namespace SMS.Application.Enrollments.Commands.AssignGrade;

public record AssignGradeCommand(Guid EnrollmentId, string Grade) : IRequest;