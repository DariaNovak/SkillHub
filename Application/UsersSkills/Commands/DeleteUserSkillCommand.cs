using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.UsersSkills.Exceptions;
using Domain.Users;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.UsersSkills.Commands;

public record DeleteUserSkillCommand : IRequest<Either<UserSkillException, bool>>
{
    public required UserId UserId { get; init; }
    public required SkillId SkillId { get; init; }
}

public class DeleteUserSkillCommandHandler(
    IUserSkillQueries userSkillQueries,
    IUserSkillRepository userSkillRepository)
    : IRequestHandler<DeleteUserSkillCommand, Either<UserSkillException, bool>>
{
    public async Task<Either<UserSkillException, bool>> Handle(
        DeleteUserSkillCommand request,
        CancellationToken cancellationToken)
    {
        var existingUserSkill = await userSkillQueries.GetByIdAsync(request.UserId, request.SkillId, cancellationToken);

        return await existingUserSkill.MatchAsync(
            us => DeleteEntity(request, cancellationToken),
            () => Task.FromResult<Either<UserSkillException, bool>>(
                new UserSkillNotFoundException(request.UserId, request.SkillId)));
    }

    private async Task<Either<UserSkillException, bool>> DeleteEntity(
        DeleteUserSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await userSkillRepository.DeleteAsync(request.UserId, request.SkillId, cancellationToken);
            return true;
        }
        catch (Exception exception)
        {
            return new UnhandledUserSkillException(request.UserId, request.SkillId, exception);
        }
    }
}
