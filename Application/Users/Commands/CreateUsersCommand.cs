using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Roles;
using Domain.Users;
using LanguageExt;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand : IRequest<Either<UserException, User>>
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public required RoleId RoleId { get; init; }
    public required DateTime JoinDate { get; init; }
}

public class CreateUserCommandHandler(
    IUserQueries userQueries,
    IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Either<UserException, User>>
{
    public async Task<Either<UserException, User>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await userQueries.GetByEmailAsync(request.Email, cancellationToken);

        return await existingUser.MatchAsync(
            u => new UserAlreadyExistException(u.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<UserException, User>> CreateEntity(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.AddAsync(
                User.New( request.Name, request.Email, request.PasswordHash, request.RoleId, request.JoinDate),
                cancellationToken);

            return user;
        }
        catch (Exception exception)
        {
            return new UnhandledUserException(UserId.Empty(), exception);
        }
    }
}