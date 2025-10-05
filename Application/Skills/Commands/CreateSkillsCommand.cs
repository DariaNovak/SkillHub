using Application.Common.Interfaces.Repositories;
using Domain.Skills;
using MediatR;

namespace Application.Skills.Commands;

public record CreateSkillsCommand : IRequest<Skill>
{
    public required string Name { get; init; }
}

public class CreateSkillsCommandHandler(
    ISkillRepository skillRepository) : IRequestHandler<CreateSkillsCommand, Skill>
{
    public async Task<Skill> Handle(CreateSkillsCommand request, CancellationToken cancellationToken)
    {
        var skill = await skillRepository.AddAsync(
            Skill.New(request.Name),
            cancellationToken);

        return skill;
    }
}
