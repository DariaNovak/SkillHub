using Api.Dtos;
using Application.Roles.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Roles;

public class DeleteRoleEndpoint : Endpoint<DeleteRoleDto>
{
    private readonly IMediator _mediator;

    public DeleteRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/roles/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteRoleDto req, CancellationToken ct)
    {
        var command = new DeleteRoleCommand(req.Id);

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}