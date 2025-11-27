using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseSkillRepository
{
    Task<CourseSkill> AddAsync(CourseSkill courseSkill, CancellationToken cancellationToken = default);
    Task DeleteAsync(CourseId courseId, SkillId skillId, CancellationToken cancellationToken = default);
}
