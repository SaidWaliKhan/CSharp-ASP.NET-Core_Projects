using Microsoft.EntityFrameworkCore;
using SMS.Application.Common.Interfaces;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context) { }

    public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

    public async Task<Student?> GetWithEnrollmentsAsync(Guid studentId, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(s => s.Email == email, cancellationToken);
}