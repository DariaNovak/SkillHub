using Api.Dtos;
using Application.Roles.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Roles;

public class UpdateRoleEndpoint : Endpoint<UpdateRoleDto>
{
    private readonly IMediator _mediator;

    public UpdateRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/roles/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateRoleDto req, CancellationToken ct)
    {
        var command = new UpdateRoleCommand(req.Id, req.Name);

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}