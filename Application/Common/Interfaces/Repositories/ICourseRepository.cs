using Domain.Courses;

namespace Application.Common.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<Course> AddAsync(Course entity, CancellationToken cancellationToken);
        Task UpdateAsync(Course entity, CancellationToken cancellationToken);
        Task DeleteAsync(CourseId id, CancellationToken cancellationToken);
    }
}
