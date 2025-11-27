using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Profiles.Exceptions;
using Domain.Profiles;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Profiles.Commands;

public record CreateProfileCommand : IRequest<Either<ProfileException, Profile>>
{
    public required UserId UserId { get; init; }
    public required string Bio { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Location { get; init; }
    public required string Website { get; init; }
}

public class CreateProfileCommandHandler(
    IProfileQueries profileQueries,
    IProfileRepository profileRepository) : IRequestHandler<CreateProfileCommand, Either<ProfileException, Profile>>
{
    public async Task<Either<ProfileException, Profile>> Handle(
        CreateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var existingProfile = await profileQueries.GetByUserIdAsync(request.UserId.Value, cancellationToken);

        return await existingProfile.MatchAsync(
            p => new UserAlreadyHasProfileException(p.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<ProfileException, Profile>> CreateEntity(
        CreateProfileCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var profile = await profileRepository.AddAsync(
                Profile.New(
                    request.UserId,
                    request.Bio,
                    request.PhoneNumber,
                    request.Location,
                    request.Website),
                cancellationToken);

            return profile;
        }
        catch (Exception exception)
        {
            return new UnhandledProfileException(ProfileId.Empty(), exception);
        }
    }
}
