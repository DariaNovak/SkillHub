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

public record UpdateRoleCommand(
    Guid Id,
    string Name
 ) : IRequest;


public class UpdateRoleCommandHandler(
    IRoleQueries roleQueries,
    IRoleRepository roleRepository) : IRequestHandler<UpdateRoleCommand>
{
    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleQueries.GetByIdAsync(request.Id, cancellationToken);

        if (role is null)
            throw new KeyNotFoundException("Role not found.");

        role.UpdateInfo(request.Name);

        await roleRepository.UpdateAsync(role, cancellationToken);

    }
}
