using Application.Common.Interfaces.Repositories;
using Domain.Roles;
using Domain.Roles.Role;
using Domain.Users;
using MediatR;

namespace Application.Users.Commands;

public record UpdateUserCommand(
    Guid UserId,
    string Name,
    string Email,
    string PasswordHash,
    Role Role,
    DateTime JoinDate
) : IRequest;

public class UpdateUserCommandHandler(
    IUserRepository userRepository) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.UserId,
            request.Name,
            request.Email,
            request.PasswordHash,
            request.Role,
            request.JoinDate
        );

        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
