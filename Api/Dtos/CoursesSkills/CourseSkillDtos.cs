using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;

namespace Api.Dtos;

public record CourseSkillDto(
    CourseId CourseId,
    SkillId SkillId)
{
    public static CourseSkillDto FromDomainModel(CourseSkill courseSkill)
        => new(
            courseSkill.CourseId,
            courseSkill.SkillId);
}

public record CreateCourseSkillDto(
    Guid CourseId,
    Guid SkillId);

public record DeleteCourseSkillDto(
    Guid CourseId,
    Guid SkillId);

public record GetCourseSkillByIdDto(
    Guid CourseId,
    Guid SkillId);

public record GetCourseSkillsByCourseIdDto(Guid CourseId);
