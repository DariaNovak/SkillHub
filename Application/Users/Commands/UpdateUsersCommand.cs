using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Roles;
using Domain.Users;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Users.Commands;

public record UpdateUserCommand : IRequest<Either<UserException, User>>
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public required RoleId RoleId { get; init; }
    public required DateTime JoinDate { get; init; }
}

public class UpdateUserCommandHandler(
    IUserQueries userQueries,
    IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, Either<UserException, User>>
{
    public async Task<Either<UserException, User>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

        var user = await userQueries.GetByIdAsync(userId, cancellationToken);

        return await user.MatchAsync(
            p => CheckDuplicates(p.Id, request.Email, cancellationToken)
                .BindAsync(_ => UpdateEntity(request, p, cancellationToken)),
            () => new UserNotFoundException(userId));
    }

    private async Task<Either<UserException, User>> UpdateEntity(
        UpdateUserCommand request,
        User user,
        CancellationToken cancellationToken)
    {
        try
        {
            user.UpdateInfo(request.Name, request.Email, request.PasswordHash, request.RoleId, request.JoinDate);

            await userRepository.UpdateAsync(user, cancellationToken);
            return user;
        }
        catch (Exception exception)
        {
            return new UnhandledUserException(user.Id, exception);
        }
    }

    private async Task<Either<UserException, Unit>> CheckDuplicates(
        UserId currentUserId,
        string email,
        CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByEmailAsync(email, cancellationToken);

        return user.Match<Either<UserException, Unit>>(
            p => p.Id.Equals(currentUserId) ? Unit.Default : new UserAlreadyExistException(p.Id),
            () => Unit.Default);
    }
}