using MediatR;
using SMS.Application.Common.Interfaces;

namespace SMS.Application.Enrollments.Commands.AssignGrade;

public class AssignGradeCommandHandler : IRequestHandler<AssignGradeCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignGradeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AssignGradeCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(request.EnrollmentId, cancellationToken);

        if (enrollment is null)
            throw new KeyNotFoundException($"Enrollment with Id '{request.EnrollmentId}' was not found.");

        enrollment.AssignGrade(request.Grade);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}