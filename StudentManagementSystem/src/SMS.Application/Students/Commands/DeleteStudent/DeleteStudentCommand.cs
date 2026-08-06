using MediatR;

namespace SMS.Application.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(Guid Id) : IRequest;