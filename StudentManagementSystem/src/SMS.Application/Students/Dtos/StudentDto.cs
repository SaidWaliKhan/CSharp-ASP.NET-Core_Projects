namespace SMS.Application.Students.Dtos;

public record StudentDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime EnrollmentDate);