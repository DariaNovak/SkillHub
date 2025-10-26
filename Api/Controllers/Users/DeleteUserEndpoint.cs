using Api.Dtos;
using Application.Users.Commands;
using FastEndpoints;
using MediatR;

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
        Delete("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserDto req, CancellationToken ct)
    {
        var command = new DeleteUserCommand { UserId = req.Id };

        await _mediator.Send(command, ct);
        Response = null;
    }
}