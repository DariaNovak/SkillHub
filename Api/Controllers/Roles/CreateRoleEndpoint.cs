using FastEndpoints;
using MediatR;
using Application.Roles.Commands;
using Api.Dtos;

namespace Api.Controllers.Roles;

public class CreateRoleEndpoint : Endpoint<CreateRoleCommand, RoleDto>
{
    private readonly IMediator _mediator;

    public CreateRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/roles");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateRoleCommand req, CancellationToken ct)
    {
        var role = await _mediator.Send(req, ct);
        Response = RoleDto.FromDomainModel(role);
    }
}