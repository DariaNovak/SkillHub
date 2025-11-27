using Application.Common.Interfaces.Queries;
using Domain.UsersSkills;
using Domain.Users;
using MediatR;

namespace Application.UsersSkills.Queries;

public record GetUserSkillsByUserIdQuery : IRequest<List<UserSkill>>
{
    public required UserId UserId { get; init; }
}

public class GetUserSkillsByUserIdQueryHandler(IUserSkillQueries userSkillQueries)
    : IRequestHandler<GetUserSkillsByUserIdQuery, List<UserSkill>>
{
    public async Task<List<UserSkill>> Handle(
        GetUserSkillsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        return await userSkillQueries.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
