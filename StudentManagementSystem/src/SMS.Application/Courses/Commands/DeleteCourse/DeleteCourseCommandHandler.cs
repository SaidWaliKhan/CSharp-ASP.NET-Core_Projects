using MediatR;
using SMS.Application.Common.Interfaces;

namespace SMS.Application.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(request.Id, cancellationToken);

        if (course is null)
            throw new KeyNotFoundException($"Course with Id '{request.Id}' was not found.");

        _unitOfWork.Courses.Remove(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}