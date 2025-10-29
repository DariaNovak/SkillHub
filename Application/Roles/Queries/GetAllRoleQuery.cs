using Application.Common.Interfaces.Queries;
using Domain.Roles.Role;
using LanguageExt;
using MediatR;

namespace Application.Roles.Queries;

public record GetAllRoleQuery : IRequest<IReadOnlyList<Role>>;

public class GetAllRoleQueryHandler(
    IRoleQueries roleQueries)
    : IRequestHandler<GetAllRoleQuery, IReadOnlyList<Role>>
{
    public async Task<IReadOnlyList<Role>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var option = await roleQueries.GetAllAsync(cancellationToken);

        return option.Match(
            Some: roles => roles,
            None: () => Array.Empty<Role>()
        );
    }
}