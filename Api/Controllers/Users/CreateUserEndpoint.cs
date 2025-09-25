using FastEndpoints;
using MediatR;
using Application.Users.Commands;
using Domain.Users;

namespace Api.Controllers.Users;

public class CreateUserEndpoint : Endpoint<CreateUserCommand, User>
{
    private readonly IMediator _mediator;

    public CreateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var user = await _mediator.Send(req, ct);
        Response = user;
    }
}