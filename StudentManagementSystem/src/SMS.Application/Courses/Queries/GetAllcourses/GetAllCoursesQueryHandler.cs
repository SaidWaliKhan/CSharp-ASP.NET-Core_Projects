using MediatR;
using SMS.Application.Common.Interfaces;
using SMS.Application.Courses.Dtos;

namespace SMS.Application.Courses.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IReadOnlyList<CourseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _unitOfWork.Courses.GetAllAsync(cancellationToken);

        return courses
            .Select(c => new CourseDto(c.Id, c.Title, c.Credits, c.MaxCapacity))
            .ToList();
    }
}