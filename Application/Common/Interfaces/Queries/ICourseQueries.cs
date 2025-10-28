using Domain.Courses;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface ICourseQueries
    {
        Task<Option<Course>> GetByIdAsync(CourseId id, CancellationToken cancellationToken);
        Task<Option<IReadOnlyList<Course>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
