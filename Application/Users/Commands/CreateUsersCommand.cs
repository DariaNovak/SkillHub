using Application.Common.Interfaces.Repositories;
using Domain.Roles;
using Domain.Roles.Role;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand : IRequest<User>
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public required Guid RoleId { get; init; }
    public required DateTime JoinDate { get; init; }
}

public class CreateUserCommandHandler(
    IUserRepository userRepository) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.AddAsync(
            User.New(Guid.NewGuid(), request.Name, request.Email,
            request.PasswordHash, request.RoleId, request.JoinDate),
            cancellationToken);

        return user;
    }
}
