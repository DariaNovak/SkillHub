using Application.Common.Interfaces.Queries;
using Domain.CoursesSkills;
using Domain.Courses;
using MediatR;

namespace Application.CoursesSkills.Queries;

public record GetCourseSkillsByCourseIdQuery : IRequest<List<CourseSkill>>
{
    public required CourseId CourseId { get; init; }
}

public class GetCourseSkillsByCourseIdQueryHandler(ICourseSkillQueries courseSkillQueries)
    : IRequestHandler<GetCourseSkillsByCourseIdQuery, List<CourseSkill>>
{
    public async Task<List<CourseSkill>> Handle(
        GetCourseSkillsByCourseIdQuery request,
        CancellationToken cancellationToken)
    {
        return await courseSkillQueries.GetByCourseIdAsync(request.CourseId, cancellationToken);
    }
}
