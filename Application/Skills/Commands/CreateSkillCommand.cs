using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Skills.Exceptions;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.Skills.Commands;

public record CreateSkillCommand : IRequest<Either<SkillException, Skill>>
{
    public required SkillId Id { get; init; }
    public required string Name { get; init; }
}

public class CreateSkillsCommandHandler(
    ISkillQueries skillQueries,
    ISkillRepository skillRepository)
    : IRequestHandler<CreateSkillCommand, Either<SkillException, Skill>>
{
    public async Task<Either<SkillException, Skill>> Handle(
        CreateSkillCommand request,
        CancellationToken cancellationToken)
    {
        var existingSkill = await skillQueries.GetByIdAsync(request.Id, cancellationToken);

        return await existingSkill.MatchAsync(
            s => new SkillAlreadyExistsException(request.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<SkillException, Skill>> CreateEntity(
        CreateSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var skill = await skillRepository.AddAsync(
                Skill.New(request.Name),
                cancellationToken);

            return skill;
        }
        catch (Exception exception)
        {
            return new UnhandledSkillException(request.Id, exception);
        }
    }
}