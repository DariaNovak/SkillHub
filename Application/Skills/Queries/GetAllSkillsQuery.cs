using Application.Common.Interfaces.Queries;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.Skills.Queries;

public record GetAllSkillsQuery : IRequest<IReadOnlyList<Skill>>;

public class GetAllSkillsQueryHandler(
    ISkillQueries skillQueries)
    : IRequestHandler<GetAllSkillsQuery, IReadOnlyList<Skill>>
{
    public async Task<IReadOnlyList<Skill>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        var option = await skillQueries.GetAllAsync(cancellationToken);

        return option.Match(
            Some: skills => skills,
            None: () => Array.Empty<Skill>()
        );
    }
}