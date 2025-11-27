using Domain.Courses;
using Domain.Skills;

namespace Application.CoursesSkills.Exceptions;

public abstract class CourseSkillException(CourseId courseId, SkillId skillId, string message)
    : Exception(message)
{
    public CourseId CourseId { get; } = courseId;
    public SkillId SkillId { get; } = skillId;
}

public class CourseSkillNotFoundException(CourseId courseId, SkillId skillId)
    : CourseSkillException(courseId, skillId, $"CourseSkill with CourseId '{courseId}' and SkillId '{skillId}' not found");

public class CourseSkillAlreadyExistsException(CourseId courseId, SkillId skillId)
    : CourseSkillException(courseId, skillId, $"CourseSkill with CourseId '{courseId}' and SkillId '{skillId}' already exists");

public class UnhandledCourseSkillException(CourseId courseId, SkillId skillId, Exception innerException)
    : CourseSkillException(courseId, skillId, $"An unhandled error occurred for CourseSkill with CourseId '{courseId}' and SkillId '{skillId}'")
{
    public Exception InnerException { get; } = innerException;
}
