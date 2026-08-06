using MediatR;
using SMS.Application.Common.Interfaces;
using SMS.Application.Students.Dtos;

namespace SMS.Application.Students.Queries.GetAllStudents;

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IReadOnlyList<StudentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllStudentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _unitOfWork.Students.GetAllAsync(cancellationToken);

        return students
            .Select(s => new StudentDto(s.Id, s.FirstName, s.LastName, s.Email, s.EnrollmentDate))
            .ToList();
    }
}