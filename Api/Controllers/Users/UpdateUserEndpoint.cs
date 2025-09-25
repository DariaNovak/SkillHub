using FastEndpoints;
using MediatR;
using Application.Users.Commands;

namespace Api.Controllers.Users;

public class UpdateUserEndpoint : Endpoint<UpdateUserCommand>
{
    private readonly IMediator _mediator;

    public UpdateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/users/{UserId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        //replace with no content
        Response = StatusCodes.Status204NoContent;
    }
}