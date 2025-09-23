using Application.Common.Interfaces.Queries;
using Domain.Users;
using MediatR;

namespace Application.Users.Queries;

public record GetUserByIdQuery(Guid UserId) : IRequest<User?>;

public class GetUserByIdQueryHandler(
    IUserQueries userRepository) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync(request.UserId, cancellationToken);
    }
}
