using Api.Dtos;
using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Users;

public class UpdateUserEndpoint : Endpoint<UpdateUserDto>
{
    private readonly IMediator _mediator;

    public UpdateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserDto req, CancellationToken ct)
    {
        var command = new UpdateUserCommand(
            req.Id,
            req.Name,
            req.Email,
            req.PasswordHash,
            req.RoleId,
            req.JoinDate
        );

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}
