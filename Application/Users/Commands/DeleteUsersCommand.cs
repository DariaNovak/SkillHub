using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Users;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Users.Commands;

public record DeleteUserCommand : IRequest<Either<UserException, Unit>>
{
    public required Guid UserId { get; init; }
}

public class DeleteUserCommandHandler(
    IUserQueries userQueries,
    IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Either<UserException, Unit>>
{
    public async Task<Either<UserException, Unit>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await userQueries.GetByIdAsync(userId, cancellationToken);

        return await user.MatchAsync(
            u => DeleteEntity(u.Id, cancellationToken),
            () => new UserNotFoundException(userId));
    }

    private async Task<Either<UserException, Unit>> DeleteEntity(
        UserId id,
        CancellationToken cancellationToken)
    {
        try
        {
            await userRepository.DeleteAsync(id, cancellationToken);
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new UnhandledUserException(id, exception);
        }
    }
}