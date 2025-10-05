using Application.Common.Interfaces.Queries;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Queries;

public record GetAllSkillsQuery() : IRequest<IReadOnlyList<Skill>>;

public class GetAllSkillsQueryHandler(
    ISkillQueries skillRepository) : IRequestHandler<GetAllSkillsQuery, IReadOnlyList<Skill>>
{
    public async Task<IReadOnlyList<Skill>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        return await skillRepository.GetAllAsync(cancellationToken);
    }
}