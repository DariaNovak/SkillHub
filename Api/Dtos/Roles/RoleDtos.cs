using Domain.Roles.Role;

namespace Api.Dtos;

public record RoleDto(
    Guid Id,
    string Name)
{
    public static RoleDto FromDomainModel(Role role)
        => new(
            role.Id,
            role.Name);
}

public record CreateRoleDto(
    string Name);

public record UpdateRoleDto(
    Guid Id,
    string Name);

public record DeleteRoleDto(Guid Id);

public record GetRoleByIdDto(Guid Id);
