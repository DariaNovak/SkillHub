using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using Domain.Roles.Role;
using LanguageExt;
using MediatR;

namespace Application.Roles.Commands;

public record UpdateRoleCommand : IRequest<Either<RoleException, Role>>
{
    public required RoleId RoleId { get; init; }
    public required string Name { get; init; }
}

public class UpdateRoleCommandHandler(
    IRoleQueries roleQueries,
    IRoleRepository roleRepository)
    : IRequestHandler<UpdateRoleCommand, Either<RoleException, Role>>
{
    public async Task<Either<RoleException, Role>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(request.RoleId, cancellationToken);

        return await role.MatchAsync(
            r => UpdateEntity(request, r, cancellationToken),
            () => new RoleNotFoundException(request.RoleId));
    }

    private async Task<Either<RoleException, Role>> UpdateEntity(
        UpdateRoleCommand request,
        Role role,
        CancellationToken cancellationToken)
    {
        try
        {
            role.UpdateInfo(request.Name);
            await roleRepository.UpdateAsync(role, cancellationToken);
            return role;
        }
        catch (Exception exception)
        {
            return new UnhandledRoleException(role.Id, exception);
        }
    }
}