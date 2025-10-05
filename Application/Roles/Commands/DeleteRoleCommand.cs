using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Roles.Commands;

public record DeleteRoleCommand(Guid RoleId) : IRequest;

public class DeleteRoleCommandHandler(
    IRoleRepository roleRepository) : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await roleRepository.DeleteAsync(request.RoleId, cancellationToken);
    }
}
