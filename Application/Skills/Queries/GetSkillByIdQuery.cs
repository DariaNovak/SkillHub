using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Queries;

public record GetSkillByIdQuery(Guid SkillId) : IRequest<Skill?>;

public class GetSkillByIdQueryHandler(
    ISkillQueries skillRepository) : IRequestHandler<GetSkillByIdQuery, Skill?>
{
    public async Task<Skill?> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        return await skillRepository.GetByIdAsync(request.SkillId, cancellationToken);
    }
}
