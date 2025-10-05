using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Commands;

public record UpdateSkillsCommand : IRequest<Skill>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateSkillsCommandHandler(
    ISkillQueries skillQueries,
    ISkillRepository skillRepository) : IRequestHandler<UpdateSkillsCommand, Skill>
{
    public async Task<Skill> Handle(UpdateSkillsCommand request, CancellationToken cancellationToken)
    {
        var skill = await skillQueries.GetByIdAsync(request.Id, cancellationToken);
        if (skill is null)
            throw new KeyNotFoundException("Skill not found.");

        skill.GetType().GetProperty("Name")!.SetValue(skill, request.Name);

        await skillRepository.UpdateAsync(skill, cancellationToken);

        return skill;
    }
}
