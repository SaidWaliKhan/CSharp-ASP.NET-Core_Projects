using MediatR;

namespace SMS.Application.Students.Commands.UpdateStudent;

public record UpdateStudentCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email) : IRequest;