using MediatR;
using SMS.Application.Common.Interfaces;

namespace SMS.Application.Enrollments.Commands.EnrollStudent;

public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public EnrollStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetWithEnrollmentsAsync(request.StudentId, cancellationToken);
        if (student is null)
            throw new KeyNotFoundException($"Student with Id '{request.StudentId}' was not found.");

        var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            throw new KeyNotFoundException($"Course with Id '{request.CourseId}' was not found.");

        var hasCapacity = await _unitOfWork.Courses.HasAvailableCapacityAsync(
            course.Id, course.MaxCapacity, cancellationToken);

        if (!hasCapacity)
            throw new InvalidOperationException($"Course '{course.Title}' has reached its maximum capacity.");

        student.EnrollIn(course);

        // No Update() call — student is already tracked (loaded via this same DbContext),
        // so EF's change tracker picks up the new Enrollment automatically on SaveChanges.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var enrollment = student.Enrollments.Single(e => e.CourseId == course.Id);
        return enrollment.Id;
    }
}    