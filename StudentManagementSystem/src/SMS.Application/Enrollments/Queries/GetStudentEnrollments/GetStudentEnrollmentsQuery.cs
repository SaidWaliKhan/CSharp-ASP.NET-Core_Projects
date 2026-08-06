using MediatR;
using SMS.Application.Enrollments.Dtos;

namespace SMS.Application.Enrollments.Queries.GetStudentEnrollments;

public record GetStudentEnrollmentsQuery(Guid StudentId) : IRequest<IReadOnlyList<EnrollmentDto>>;