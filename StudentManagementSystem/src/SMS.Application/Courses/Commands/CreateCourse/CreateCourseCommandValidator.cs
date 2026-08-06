using FluentValidation;

namespace SMS.Application.Courses.Commands.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Course title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Credits)
            .GreaterThan(0).WithMessage("Credits must be greater than zero.")
            .LessThanOrEqualTo(10).WithMessage("Credits must be realistic (max 10).");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than zero.")
            .LessThanOrEqualTo(1000).WithMessage("Max capacity must be realistic (max 1000).");
    
}
}