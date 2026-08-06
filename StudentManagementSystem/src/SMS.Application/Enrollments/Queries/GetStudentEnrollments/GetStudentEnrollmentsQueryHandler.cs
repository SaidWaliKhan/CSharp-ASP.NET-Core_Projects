using MediatR;
using SMS.Application.Common.Interfaces;
using SMS.Application.Enrollments.Dtos;

namespace SMS.Application.Enrollments.Queries.GetStudentEnrollments;

public class GetStudentEnrollmentsQueryHandler
    : IRequestHandler<GetStudentEnrollmentsQuery, IReadOnlyList<EnrollmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentEnrollmentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EnrollmentDto>> Handle(
        GetStudentEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId, cancellationToken);
        if (student is null)
            throw new KeyNotFoundException($"Student with Id '{request.StudentId}' was not found.");

        var enrollments = await _unitOfWork.Enrollments.GetByStudentIdAsync(request.StudentId, cancellationToken);

        var result = new List<EnrollmentDto>();
        foreach (var e in enrollments)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(e.CourseId, cancellationToken);
            result.Add(new EnrollmentDto(
                e.Id, student.Id, $"{student.FirstName} {student.LastName}",
                e.CourseId, course!.Title, e.EnrolledAtUtc, e.Grade));
        }

        return result;
    }
}