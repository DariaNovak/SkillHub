using Api.Dtos;
using Application.Roles.Commands;
using FastEndpoints;
using MediatR;
using LanguageExt;
using Unit = LanguageExt.Unit;

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
        var command = new DeleteRoleCommand { RoleId = req.Id };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async _ => await Send.NoContentAsync(ct),
            Left: async _ => await Send.NotFoundAsync(ct)
        );
    }
}