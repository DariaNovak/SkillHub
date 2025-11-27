using Application.Common.Interfaces.Queries;
using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.UsersSkills.Queries;

public record GetUserSkillByIdQuery : IRequest<Option<UserSkill>>
{
    public required UserId UserId { get; init; }
    public required SkillId SkillId { get; init; }
}

public class GetUserSkillByIdQueryHandler(IUserSkillQueries userSkillQueries)
    : IRequestHandler<GetUserSkillByIdQuery, Option<UserSkill>>
{
    public async Task<Option<UserSkill>> Handle(
        GetUserSkillByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await userSkillQueries.GetByIdAsync(request.UserId, request.SkillId, cancellationToken);
    }
}
