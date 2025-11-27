using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Profiles.Exceptions;
using Domain.Profiles;
using LanguageExt;
using MediatR;

namespace Application.Profiles.Commands;

public record UpdateProfileCommand : IRequest<Either<ProfileException, Profile>>
{
    public required Guid ProfileId { get; init; }
    public required string Bio { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Location { get; init; }
    public required string Website { get; init; }
}

public class UpdateProfileCommandHandler(
    IProfileQueries profileQueries,
    IProfileRepository profileRepository) : IRequestHandler<UpdateProfileCommand, Either<ProfileException, Profile>>
{
    public async Task<Either<ProfileException, Profile>> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var existingProfile = await profileQueries.GetByIdAsync(new ProfileId(request.ProfileId), cancellationToken);

        return await existingProfile.MatchAsync(
            async profile => await UpdateEntity(profile, request, cancellationToken),
            () => Task.FromResult<Either<ProfileException, Profile>>(
                new ProfileNotFoundException(new ProfileId(request.ProfileId))));
    }

    private async Task<Either<ProfileException, Profile>> UpdateEntity(
        Profile profile,
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            profile.UpdateInfo(
                request.Bio,
                request.PhoneNumber,
                request.Location,
                request.Website);

            var updatedProfile = await profileRepository.UpdateAsync(profile, cancellationToken);
            return updatedProfile;
        }
        catch (Exception exception)
        {
            return new UnhandledProfileException(profile.Id, exception);
        }
    }
}
