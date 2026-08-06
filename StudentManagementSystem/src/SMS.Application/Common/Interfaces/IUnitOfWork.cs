namespace SMS.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}