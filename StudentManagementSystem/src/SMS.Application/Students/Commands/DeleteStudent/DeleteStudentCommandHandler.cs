using MediatR;
using SMS.Application.Common.Interfaces;

namespace SMS.Application.Students.Commands.DeleteStudent;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
            throw new KeyNotFoundException($"Student with Id '{request.Id}' was not found.");

        _unitOfWork.Students.Remove(student);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}