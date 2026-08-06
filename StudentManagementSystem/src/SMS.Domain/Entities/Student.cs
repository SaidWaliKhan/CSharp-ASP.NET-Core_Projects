using System.Runtime.InteropServices;

namespace SMS.Domain.Entities;
public class Student
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateTime EnrollmentDate { get; private set; }


    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();


    private Student() { }

    public Student(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName is required", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("A valid eamil is required", nameof(email));

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        EnrollmentDate = DateTime.UtcNow;

    }

    public void EnrollIn(Course course)
    {
        if (_enrollments.Any(e => e.CourseId == course.Id))
            throw new InvalidOperationException($"Student is already enrolled in {course.Title}.");

        _enrollments.Add(new Enrollment(this.Id, course.Id));
    }


    public void UpdateDetails(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("A valid email is required.", nameof(email));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

}