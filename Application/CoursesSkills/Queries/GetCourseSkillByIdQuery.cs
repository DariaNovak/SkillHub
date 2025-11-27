using Application.Common.Interfaces.Queries;
using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.CoursesSkills.Queries;

public record GetCourseSkillByIdQuery : IRequest<Option<CourseSkill>>
{
    public required CourseId CourseId { get; init; }
    public required SkillId SkillId { get; init; }
}

public class GetCourseSkillByIdQueryHandler(ICourseSkillQueries courseSkillQueries)
    : IRequestHandler<GetCourseSkillByIdQuery, Option<CourseSkill>>
{
    public async Task<Option<CourseSkill>> Handle(
        GetCourseSkillByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await courseSkillQueries.GetByIdAsync(request.CourseId, request.SkillId, cancellationToken);
    }
}
