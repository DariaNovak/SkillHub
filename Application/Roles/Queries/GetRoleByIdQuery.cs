using Application.Common.Interfaces.Queries;
using Domain.Roles.Role;
using MediatR;

namespace Application.Roles.Queries;

public record GetRoleByIdQuery(Guid RoleId) : IRequest<Role?>;

public class GetRoleByIdQueryHandler(
    IRoleQueries roleRepository) : IRequestHandler<GetRoleByIdQuery, Role?>
{
    public async Task<Role?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
    }
}