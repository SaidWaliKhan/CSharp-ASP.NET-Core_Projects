namespace SMS.Application.Courses.Dtos;

public record CourseDto(
    Guid Id,
    string Title,
    int Credits,
    int MaxCapacity);