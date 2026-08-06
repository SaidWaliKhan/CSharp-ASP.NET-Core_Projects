using MediatR;
using SMS.Application.Common.Interfaces;

namespace SMS.Application.Students.Commands.UpdateStudent;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
            throw new KeyNotFoundException($"Student with Id '{request.Id}' was not found.");

        student.UpdateDetails(request.FirstName, request.LastName, request.Email);

        // No Update() call needed — student is already tracked by this request's DbContext
        // since we loaded it via GetByIdAsync above. The change tracker sees the property
        // changes automatically when SaveChangesAsync runs.
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}