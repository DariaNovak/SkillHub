using Application.Common.Interfaces.Queries;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.Skills.Queries;

public record GetSkillByIdQuery(SkillId Id) : IRequest<Option<Skill>>;

public class GetSkillByIdQueryHandler(
    ISkillQueries skillQueries) : IRequestHandler<GetSkillByIdQuery, Option<Skill>>
{
    public async Task<Option<Skill>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        var option = await skillQueries.GetByIdAsync(request.Id, cancellationToken);
        return option.Match(
            Some: skill => skill,
            None: () => Option<Skill>.None
        );
    }
}