using Application.Common.Interfaces.Queries;
using Domain.Roles;
using Domain.Roles.Role;
using LanguageExt;
using MediatR;

namespace Application.Roles.Queries;

public record GetRoleByIdQuery(RoleId Id) : IRequest<Option<Role>>;

public class GetRoleByIdQueryHandler(
    IRoleQueries roleQueries) : IRequestHandler<GetRoleByIdQuery, Option<Role>>
{
    public async Task<Option<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var option = await roleQueries.GetByIdAsync(request.Id, cancellationToken);
        return option.Match(
            Some: role => role,
            None: () => Option<Role>.None
        );
    }
}