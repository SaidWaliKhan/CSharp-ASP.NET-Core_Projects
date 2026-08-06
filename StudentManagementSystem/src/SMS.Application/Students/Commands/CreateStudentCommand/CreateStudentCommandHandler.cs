using MediatR;
using SMS.Application.Common.Interfaces;
using SMS.Domain.Entities;

namespace SMS.Application.Students.Commands.CreateStudent;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var emailAlreadyExists = await _unitOfWork.Students.EmailExistsAsync(request.Email, cancellationToken);
        if (emailAlreadyExists)
            throw new InvalidOperationException($"A student with email '{request.Email}' already exists.");

        var student = new Student(request.FirstName, request.LastName, request.Email);

        await _unitOfWork.Students.AddAsync(student, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return student.Id;
    }
}