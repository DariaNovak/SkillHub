using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Roles.Role;
using MediatR;

namespace Application.Roles.Commands;

public record UpdateRoleCommand : IRequest<Role>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateRoleCommandHandler(
    IRoleQueries roleQueries,
    IRoleRepository roleRepository) : IRequestHandler<UpdateRoleCommand, Role>
{
    public async Task<Role> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(request.Id, cancellationToken);
        if (role is null)
            throw new KeyNotFoundException("Role not found.");

        role.UpdateRole(request.Name);
        await roleRepository.UpdateAsync(role, cancellationToken);

        return role;
    }
}
