using MediatR;
using SMS.Application.Students.Dtos;

namespace SMS.Application.Students.Queries.GetAllStudents;

public record GetAllStudentsQuery : IRequest<IReadOnlyList<StudentDto>>;