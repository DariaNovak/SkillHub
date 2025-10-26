using Application.Common.Interfaces.Queries;
using Domain.Users;
using MediatR;

namespace Application.Users.Queries;

public record GetAllUsersQuery : IRequest<IReadOnlyList<User>>;

public class GetAllUsersQueryHandler(IUserQueries userQueries)
    : IRequestHandler<GetAllUsersQuery, IReadOnlyList<User>>
{
    public async Task<IReadOnlyList<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var option = await userQueries.GetAllAsync(cancellationToken);

        return option.Match(
            Some: users => users,
            None: () => Array.Empty<User>()
        );
    }
}