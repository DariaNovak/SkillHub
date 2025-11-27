using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface ICourseSkillQueries
{
    Task<Option<CourseSkill>> GetByIdAsync(CourseId courseId, SkillId skillId, CancellationToken cancellationToken = default);
    Task<List<CourseSkill>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<CourseSkill>> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken = default);
}
