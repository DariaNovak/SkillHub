using Domain.Profiles;
using Domain.Users;

namespace Api.Dtos;

public record ProfileDto(
    ProfileId Id,
    UserId UserId,
    string Bio,
    string PhoneNumber,
    string Location,
    string Website,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static ProfileDto FromDomainModel(Profile profile)
        => new(
            profile.Id,
            profile.UserId,
            profile.Bio,
            profile.PhoneNumber,
            profile.Location,
            profile.Website,
            profile.CreatedAt,
            profile.UpdatedAt);
}

public record CreateProfileDto(
    Guid UserId,
    string Bio,
    string PhoneNumber,
    string Location,
    string Website);

public record UpdateProfileDto(
    Guid Id,
    string Bio,
    string PhoneNumber,
    string Location,
    string Website);

public record DeleteProfileDto(Guid Id);

public record GetProfileByIdDto(Guid Id);
