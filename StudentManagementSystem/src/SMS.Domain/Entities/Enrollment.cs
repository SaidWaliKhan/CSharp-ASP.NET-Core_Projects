namespace SMS.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; private set; }
    public Guid StudentId { get; private set; }
    public Guid CourseId { get; private set; }
    public DateTime EnrolledAtUtc { get; private set; }
    public string? Grade { get; private set; }

    private Enrollment() { }


    internal Enrollment(Guid studentId, Guid courseId)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        CourseId = courseId;
        EnrolledAtUtc = DateTime.UtcNow;
    }

    public void AssignGrade(string grade)
    {
        if (string.IsNullOrWhiteSpace(grade))
            throw new ArgumentException("Grade cannot be empty.", nameof(grade));

        Grade = grade;
    }
}