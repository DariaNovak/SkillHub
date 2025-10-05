using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string PasswordHash,
    Guid RoleId,
    DateTime JoinDate
) : IRequest;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUserQueries userQueries) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userQueries.GetByIdAsync(request.Id, cancellationToken);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.UpdateInfo(
            request.Name,
            request.Email,
            request.PasswordHash,
            request.RoleId,
            request.JoinDate
        );

        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
