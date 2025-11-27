using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Roles.Exceptions;
using Domain.Roles;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Roles.Commands;

public record DeleteRoleCommand : IRequest<Either<RoleException, Unit>>
{
    public required Guid RoleId { get; init; }
}

public class DeleteRoleCommandHandler(
    IRoleQueries roleQueries,
    IRoleRepository roleRepository)
    : IRequestHandler<DeleteRoleCommand, Either<RoleException, Unit>>
{
    public async Task<Either<RoleException, Unit>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);
        var role = await roleQueries.GetByIdAsync(roleId, cancellationToken);

        return await role.MatchAsync(
            r => DeleteEntity(r.Id, cancellationToken),
            () => new RoleNotFoundException(roleId));
    }

    private async Task<Either<RoleException, Unit>> DeleteEntity(
        RoleId id,
        CancellationToken cancellationToken)
    {
        try
        {
            await roleRepository.DeleteAsync(id.Value, cancellationToken);
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new UnhandledRoleException(id, exception);
        }
    }
}