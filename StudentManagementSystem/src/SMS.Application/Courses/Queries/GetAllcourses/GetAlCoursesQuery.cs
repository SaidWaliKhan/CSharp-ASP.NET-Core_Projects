using MediatR;
using SMS.Application.Courses.Dtos;

namespace SMS.Application.Courses.Queries.GetAllCourses;

public record GetAllCoursesQuery : IRequest<IReadOnlyList<CourseDto>>;