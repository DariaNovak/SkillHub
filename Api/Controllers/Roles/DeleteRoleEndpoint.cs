using FastEndpoints;
using MediatR;
using Application.Roles.Commands;

namespace Api.Controllers.Roles;

public class DeleteRoleEndpoint : Endpoint<DeleteRoleCommand>
{
    private readonly IMediator _mediator;

    public DeleteRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/roles/{RoleId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteRoleCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        Response = StatusCodes.Status204NoContent;
    }
}