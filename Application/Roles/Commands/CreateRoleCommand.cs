using Application.Common.Interfaces.Repositories;
using Domain.Roles.Role;
using MediatR;

namespace Application.Roles.Commands
{
    public record CreateRoleCommand : IRequest<Role>
    {
        public required string Name { get; init; }
    }

    public class CreateRoleCommandHandler(
        IRoleRepository roleRepository) : IRequestHandler<CreateRoleCommand, Role>
    {
        public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = Role.New(Guid.NewGuid(), request.Name);
            await roleRepository.AddAsync(role, cancellationToken);
            return role;
        }
    }
}
