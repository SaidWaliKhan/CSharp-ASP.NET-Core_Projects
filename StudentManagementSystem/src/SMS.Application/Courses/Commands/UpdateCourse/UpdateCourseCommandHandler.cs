using MediatR;
using SMS.Application.Common.Interfaces;

namespace SMS.Application.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(request.Id, cancellationToken);

        if (course is null)
            throw new KeyNotFoundException($"Course with Id '{request.Id}' was not found.");

        course.UpdateDetails(request.Title, request.Credits, request.MaxCapacity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}