using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.UsersSkills.Exceptions;
using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using LanguageExt;
using MediatR;

namespace Application.UsersSkills.Commands;

public record CreateUserSkillCommand : IRequest<Either<UserSkillException, UserSkill>>
{
    public required UserId UserId { get; init; }
    public required SkillId SkillId { get; init; }
    public required int ProficiencyLevel { get; init; }
}

public class CreateUserSkillCommandHandler(
    IUserSkillQueries userSkillQueries,
    IUserSkillRepository userSkillRepository)
    : IRequestHandler<CreateUserSkillCommand, Either<UserSkillException, UserSkill>>
{
    public async Task<Either<UserSkillException, UserSkill>> Handle(
        CreateUserSkillCommand request,
        CancellationToken cancellationToken)
    {
        var existingUserSkill = await userSkillQueries.GetByIdAsync(request.UserId, request.SkillId, cancellationToken);

        return await existingUserSkill.MatchAsync(
            us => new UserSkillAlreadyExistsException(request.UserId, request.SkillId),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<UserSkillException, UserSkill>> CreateEntity(
        CreateUserSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userSkill = await userSkillRepository.AddAsync(
                UserSkill.New(request.UserId, request.SkillId, request.ProficiencyLevel),
                cancellationToken);

            return userSkill;
        }
        catch (Exception exception)
        {
            return new UnhandledUserSkillException(request.UserId, request.SkillId, exception);
        }
    }
}
