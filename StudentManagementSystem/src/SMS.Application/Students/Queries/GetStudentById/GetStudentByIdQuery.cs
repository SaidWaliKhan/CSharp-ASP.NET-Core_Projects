using MediatR;
using SMS.Application.Students.Dtos;

namespace SMS.Application.Students.Queries.GetStudentById;

public record GetStudentByIdQuery(Guid Id) : IRequest<StudentDto>;