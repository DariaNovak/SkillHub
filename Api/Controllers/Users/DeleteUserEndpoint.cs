using FastEndpoints;
using MediatR;
using Application.Users.Commands;
using Api.Dtos;

namespace Api.Controllers.Users;

public class DeleteUserEndpoint : Endpoint<DeleteUserDto>
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

    public override async Task HandleAsync(DeleteUserDto req, CancellationToken ct)
    {
        var command = new DeleteUserCommand(req.Id);
        await _mediator.Send(command, ct);
        Response = StatusCodes.Status204NoContent;
    }
}