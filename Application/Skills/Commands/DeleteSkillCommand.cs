using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Skills.Exceptions;
using Domain.Skills;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Skills.Commands;

public record DeleteSkillCommand : IRequest<Either<SkillException, Unit>>
{
    public required Guid SkillId { get; init; }
}

public class DeleteSkillCommandHandler(
    ISkillQueries skillQueries,
    ISkillRepository skillRepository)
    : IRequestHandler<DeleteSkillCommand, Either<SkillException, Unit>>
{
    public async Task<Either<SkillException, Unit>> Handle(
        DeleteSkillCommand request,
        CancellationToken cancellationToken)
    {
        var skillId = new SkillId(request.SkillId);
        var skill = await skillQueries.GetByIdAsync(skillId, cancellationToken);

        return await skill.MatchAsync(
            s => DeleteEntity(s.Id, cancellationToken),
            () => new SkillNotFoundException(skillId));
    }

    private async Task<Either<SkillException, Unit>> DeleteEntity(
        SkillId id,
        CancellationToken cancellationToken)
    {
        try
        {
            await skillRepository.DeleteAsync(id.Value, cancellationToken);
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new UnhandledSkillException(id, exception);
        }
    }
}