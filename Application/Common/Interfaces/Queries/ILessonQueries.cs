using Domain.Lessons;

namespace Application.Common.Interfaces.Queries
{
    public interface ILessonQueries
    {
        Task<Lesson?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Lesson>> GetAllAsync(CancellationToken cancellationToken);
    }
}
