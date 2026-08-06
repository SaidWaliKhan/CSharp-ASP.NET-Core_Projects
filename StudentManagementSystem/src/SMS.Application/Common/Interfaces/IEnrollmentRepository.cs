using SMS.Domain.Entities;

namespace SMS.Application.Common.Interfaces;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<IReadOnlyList<Enrollment>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Enrollment>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
}