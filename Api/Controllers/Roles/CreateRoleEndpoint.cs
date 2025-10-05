using Api.Dtos;
using Application.Roles.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Roles;

public class CreateRoleEndpoint : Endpoint<CreateRoleDto, RoleDto>
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

    public override async Task HandleAsync(CreateRoleDto req, CancellationToken ct)
    {
        var command = new CreateRoleCommand
        {
            Name = req.Name
        };
        var role = await _mediator.Send(command, ct);
        Response = RoleDto.FromDomainModel(role);
    }
}