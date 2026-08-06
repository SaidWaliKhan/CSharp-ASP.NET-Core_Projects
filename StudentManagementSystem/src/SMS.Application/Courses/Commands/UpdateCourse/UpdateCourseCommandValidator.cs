using FluentValidation;

namespace SMS.Application.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Credits).GreaterThan(0).LessThanOrEqualTo(10);
        RuleFor(x => x.MaxCapacity).GreaterThan(0).LessThanOrEqualTo(1000);
    }
}