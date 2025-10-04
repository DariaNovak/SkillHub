using Domain.Lessons;

namespace Application.Common.Interfaces.Repositories
{
    public interface ILessonRepository
    {
        Task<Lesson> AddAsync(Lesson entity, CancellationToken cancellationToken);
        Task UpdateAsync(Lesson entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
