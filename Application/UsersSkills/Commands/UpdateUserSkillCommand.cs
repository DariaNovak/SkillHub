using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.UsersSkills.Exceptions;
using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.UsersSkills.Commands;

public record UpdateUserSkillCommand : IRequest<Either<UserSkillException, UserSkill>>
{
    public required UserId UserId { get; init; }
    public required SkillId SkillId { get; init; }
    public required int ProficiencyLevel { get; init; }
}

public class UpdateUserSkillCommandHandler(
    IUserSkillQueries userSkillQueries,
    IUserSkillRepository userSkillRepository)
    : IRequestHandler<UpdateUserSkillCommand, Either<UserSkillException, UserSkill>>
{
    public async Task<Either<UserSkillException, UserSkill>> Handle(
        UpdateUserSkillCommand request,
        CancellationToken cancellationToken)
    {
        var existingUserSkill = await userSkillQueries.GetByIdAsync(request.UserId, request.SkillId, cancellationToken);

        return await existingUserSkill.MatchAsync(
            us => UpdateEntity(us, request, cancellationToken),
            () => Task.FromResult<Either<UserSkillException, UserSkill>>(
                new UserSkillNotFoundException(request.UserId, request.SkillId)));
    }

    private async Task<Either<UserSkillException, UserSkill>> UpdateEntity(
        UserSkill userSkill,
        UpdateUserSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Since UserSkill doesn't have an UpdateInfo method, we'll create a new one
            var updatedUserSkill = UserSkill.New(request.UserId, request.SkillId, request.ProficiencyLevel);
            var result = await userSkillRepository.UpdateAsync(updatedUserSkill, cancellationToken);

            return result;
        }
        catch (Exception exception)
        {
            return new UnhandledUserSkillException(request.UserId, request.SkillId, exception);
        }
    }
}
