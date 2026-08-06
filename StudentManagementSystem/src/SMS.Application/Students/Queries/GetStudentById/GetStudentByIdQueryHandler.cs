using MediatR;
using SMS.Application.Common.Interfaces;
using SMS.Application.Students.Dtos;

namespace SMS.Application.Students.Queries.GetStudentById;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(request.Id, cancellationToken);

        if (student is null)
            throw new KeyNotFoundException($"Student with Id '{request.Id}' was not found.");

        return new StudentDto(
            student.Id,
            student.FirstName,
            student.LastName,
            student.Email,
            student.EnrollmentDate);
    }
}