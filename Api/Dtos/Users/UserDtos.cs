using Domain.Roles;
using Domain.Roles.Role;
using Domain.Users;

namespace Api.Dtos;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    string PasswordHash,
    Role Role,
    DateTime JoinDate)
{
    public static UserDto FromDomainModel(User user)
        => new(
            user.Id,
            user.Name,
            user.Email,
            user.PasswordHash,
            user.Role,
            user.JoinDate);
}

public record CreateUserDto(
    string Name,
    string Email,
    string PasswordHash,
    Role Role,
    DateTime JoinDate);

public record UpdateUserDto(
    Guid Id,
    string Name,
    string Email,
    string PasswordHash,
    Role Role,
    DateTime JoinDate);

public record DeleteUserDto(Guid Id);

public record GetUserByIdDto(Guid Id);

public record GetAllUsersDto();
