namespace SMS.Domain.Entities;

public class Course
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public int Credits { get; private set; }
    public int MaxCapacity { get; private set; }

    private Course() { }

    public Course(string title, int credits, int maxCapacity)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Course title is required.", nameof(title));

        if (credits <= 0)
            throw new ArgumentOutOfRangeException(nameof(credits), "Credits must be greater than zero.");

        if (maxCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Max capacity must be greater than zero.");

        Id = Guid.NewGuid();
        Title = title;
        Credits = credits;
        MaxCapacity = maxCapacity;
    }

    public void UpdateDetails(string title, int credits, int maxCapacity)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Course title is required.", nameof(title));

        if (credits <= 0)
            throw new ArgumentOutOfRangeException(nameof(credits), "Credits must be greater than zero.");

        if (maxCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Max capacity must be greater than zero.");

        Title = title;
        Credits = credits;
        MaxCapacity = maxCapacity;
    }
}