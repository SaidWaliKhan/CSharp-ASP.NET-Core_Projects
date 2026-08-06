using Microsoft.EntityFrameworkCore;
using SMS.Application.Common.Interfaces;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<bool> HasAvailableCapacityAsync(
        Guid courseId, int maxCapacity, CancellationToken cancellationToken = default)
    {
        var enrolledCount = await Context.Set<Enrollment>()
            .CountAsync(e => e.CourseId == courseId, cancellationToken);

        return enrolledCount < maxCapacity;
    }
}