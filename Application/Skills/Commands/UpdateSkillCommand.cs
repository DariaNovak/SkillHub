using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Skills.Exceptions;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.Skills.Commands;

public record UpdateSkillCommand : IRequest<Either<SkillException, Skill>>
{
    public required SkillId SkillId { get; init; }
    public required string Name { get; init; }
}

public class UpdateSkillCommandHandler(
    ISkillQueries skillQueries,
    ISkillRepository skillRepository)
    : IRequestHandler<UpdateSkillCommand, Either<SkillException, Skill>>
{
    public async Task<Either<SkillException, Skill>> Handle(
        UpdateSkillCommand request,
        CancellationToken cancellationToken)
    {
        var skill = await skillQueries.GetByIdAsync(request.SkillId, cancellationToken);

        return await skill.MatchAsync(
            s => UpdateEntity(request, s, cancellationToken),
            () => new SkillNotFoundException(request.SkillId));
    }

    private async Task<Either<SkillException, Skill>> UpdateEntity(
        UpdateSkillCommand request,
        Skill skill,
        CancellationToken cancellationToken)
    {
        try
        {
            skill.UpdateInfo(request.Name);
            await skillRepository.UpdateAsync(skill, cancellationToken);
            return skill;
        }
        catch (Exception exception)
        {
            return new UnhandledSkillException(skill.Id, exception);
        }
    }
}