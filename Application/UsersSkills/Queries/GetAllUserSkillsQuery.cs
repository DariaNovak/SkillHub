using Application.Common.Interfaces.Queries;
using Domain.UsersSkills;
using MediatR;

namespace Application.UsersSkills.Queries;

public record GetAllUserSkillsQuery : IRequest<List<UserSkill>>;

public class GetAllUserSkillsQueryHandler(IUserSkillQueries userSkillQueries)
    : IRequestHandler<GetAllUserSkillsQuery, List<UserSkill>>
{
    public async Task<List<UserSkill>> Handle(
        GetAllUserSkillsQuery request,
        CancellationToken cancellationToken)
    {
        return await userSkillQueries.GetAllAsync(cancellationToken);
    }
}
