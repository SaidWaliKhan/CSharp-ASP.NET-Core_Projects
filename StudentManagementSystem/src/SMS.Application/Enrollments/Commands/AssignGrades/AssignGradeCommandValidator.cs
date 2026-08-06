using FluentValidation;

namespace SMS.Application.Enrollments.Commands.AssignGrade;

public class AssignGradeCommandValidator : AbstractValidator<AssignGradeCommand>
{
    private static readonly string[] ValidGrades = { "A", "B", "C", "D", "F" };

    public AssignGradeCommandValidator()
    {
        RuleFor(x => x.EnrollmentId).NotEmpty();
        RuleFor(x => x.Grade)
            .NotEmpty()
            .Must(g => ValidGrades.Contains(g))
            .WithMessage($"Grade must be one of: {string.Join(", ", ValidGrades)}.");
    }
}