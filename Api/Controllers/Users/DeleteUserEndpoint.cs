using FastEndpoints;
using MediatR;
using Application.Users.Commands;

namespace Api.Controllers.Users;

public class DeleteUserEndpoint : Endpoint<DeleteUserCommand>
{
    private readonly IMediator _mediator;

    public DeleteUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/users/{UserId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        Response = StatusCodes.Status204NoContent;
    }
}