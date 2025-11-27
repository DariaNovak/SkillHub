using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Profiles.Exceptions;
using Domain.Profiles;
using LanguageExt;
using MediatR;

namespace Application.Profiles.Commands;

public record DeleteProfileCommand : IRequest<Either<ProfileException, Profile>>
{
    public required Guid ProfileId { get; init; }
}

public class DeleteProfileCommandHandler(
    IProfileQueries profileQueries,
    IProfileRepository profileRepository) : IRequestHandler<DeleteProfileCommand, Either<ProfileException, Profile>>
{
    public async Task<Either<ProfileException, Profile>> Handle(
        DeleteProfileCommand request,
        CancellationToken cancellationToken)
    {
        var existingProfile = await profileQueries.GetByIdAsync(new ProfileId(request.ProfileId), cancellationToken);

        return await existingProfile.MatchAsync(
            async profile => await DeleteEntity(profile, cancellationToken),
            () => Task.FromResult<Either<ProfileException, Profile>>(
                new ProfileNotFoundException(new ProfileId(request.ProfileId))));
    }

    private async Task<Either<ProfileException, Profile>> DeleteEntity(
        Profile profile,
        CancellationToken cancellationToken)
    {
        try
        {
            var deletedProfile = await profileRepository.DeleteAsync(profile, cancellationToken);
            return deletedProfile;
        }
        catch (Exception exception)
        {
            return new UnhandledProfileException(profile.Id, exception);
        }
    }
}
