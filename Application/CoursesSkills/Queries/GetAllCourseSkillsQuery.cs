using Application.Common.Interfaces.Queries;
using Domain.CoursesSkills;
using MediatR;

namespace Application.CoursesSkills.Queries;

public record GetAllCourseSkillsQuery : IRequest<List<CourseSkill>>;

public class GetAllCourseSkillsQueryHandler(ICourseSkillQueries courseSkillQueries)
    : IRequestHandler<GetAllCourseSkillsQuery, List<CourseSkill>>
{
    public async Task<List<CourseSkill>> Handle(
        GetAllCourseSkillsQuery request,
        CancellationToken cancellationToken)
    {
        return await courseSkillQueries.GetAllAsync(cancellationToken);
    }
}
