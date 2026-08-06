using SMS.Application.Common.Interfaces;
using SMS.Infrastructure.Persistence.Repositories;

namespace SMS.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IStudentRepository? _students;
    private ICourseRepository? _courses;
    private IEnrollmentRepository? _enrollments;


    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    // Lazy-loaded: repositories are only constructed the first time they're actually used,
    // not on every UnitOfWork creation — avoids wasted allocations for handlers that only need one.
    public IStudentRepository Students => _students ??= new StudentRepository(_context);
    public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
    public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}