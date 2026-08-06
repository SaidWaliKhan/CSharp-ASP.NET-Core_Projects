using SMS.Domain.Entities;

namespace SMS.Application.Common.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<bool> HasAvailableCapacityAsync(Guid courseId, int maxCapacity, CancellationToken cancellationToken = default);
}