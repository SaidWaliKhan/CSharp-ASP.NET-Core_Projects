namespace SMS.Application.Enrollments.Dtos;

public record EnrollmentDto(
    Guid Id,
    Guid StudentId,
    string StudentFullName,
    Guid CourseId,
    string CourseTitle,
    DateTime EnrolledAtUtc,
    string? Grade);