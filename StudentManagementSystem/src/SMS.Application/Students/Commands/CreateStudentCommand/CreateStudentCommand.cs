using MediatR;

namespace SMS.Application.Students.Commands.CreateStudent;

public record CreateStudentCommand(
    string FirstName,
    string LastName,
    string Email) : IRequest<Guid>;