using Domain.Courses;

namespace Application.Common.Interfaces.Queries
{
    public interface ICourseQueries
    {
        Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken);
    }
}
