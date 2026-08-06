using MediatR;
using SMS.Application.Courses.Dtos;

namespace SMS.Application.Courses.Queries.GetCourseById;

public record GetCourseByIdQuery(Guid Id) : IRequest<CourseDto>;