using Application.Common.Interfaces.Queries;
using Domain.Users;
using MediatR;

namespace Application.Users.Queries;

public record GetAllUsersQuery() : IRequest<IReadOnlyList<User>>;

public class GetAllUsersQueryHandler(
    IUserQueries userRepository) : IRequestHandler<GetAllUsersQuery, IReadOnlyList<User>>
{
    public async Task<IReadOnlyList<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAllAsync(cancellationToken);
    }
}
