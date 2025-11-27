using Domain.Lessons;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface ILessonQueries
    {
        Task<Option<Lesson?>> GetByIdAsync(LessonId id, CancellationToken cancellationToken);
        Task<Option<IReadOnlyList<Lesson>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
