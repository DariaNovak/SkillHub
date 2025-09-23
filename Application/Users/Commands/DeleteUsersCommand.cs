using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest;

public class DeleteUserCommandHandler(
    IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await userRepository.DeleteAsync(request.UserId, cancellationToken);
    }
}
