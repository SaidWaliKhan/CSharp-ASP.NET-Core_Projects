using MediatR;
using SMS.Application.Common.Interfaces;
using SMS.Application.Courses.Dtos;

namespace SMS.Application.Courses.Queries.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(request.Id, cancellationToken);

        if (course is null)
            throw new KeyNotFoundException($"Course with Id '{request.Id}' was not found.");

        return new CourseDto(course.Id, course.Title, course.Credits,course.MaxCapacity);
    }
}