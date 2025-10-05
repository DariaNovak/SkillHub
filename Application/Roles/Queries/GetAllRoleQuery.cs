using Application.Common.Interfaces.Queries;
using Domain.Roles.Role;
using MediatR;

namespace Application.Roles.Queries;

public record GetAllRoleQuery() : IRequest<IReadOnlyList<Role>>;

public class GetAllRoleQueryHandler(
    IRoleQueries roleRepository) : IRequestHandler<GetAllRoleQuery, IReadOnlyList<Role>>
{
    public async Task<IReadOnlyList<Role>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        return await roleRepository.GetAllAsync(cancellationToken);
    }
}