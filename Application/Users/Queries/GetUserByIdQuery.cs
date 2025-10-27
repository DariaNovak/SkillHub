using Application.Common.Interfaces.Queries;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Users.Queries;

public record GetUserByIdQuery(UserId Id) : IRequest<Option<User>>;

public class GetUserByIdQueryHandler(
    IUserQueries userQueries) : IRequestHandler<GetUserByIdQuery, Option<User>>
{
    public async Task<Option<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var option = await userQueries.GetByIdAsync(request.Id, cancellationToken);
        return option.Match(
            Some: user => user,
            None: () => Option<User>.None
        );
    }
}