using Api.Dtos;
using Application.Roles.Commands;
using Domain.Roles;
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
            Id = RoleId.New(),
            Name = req.Name
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async role =>
            {
                var response = RoleDto.FromDomainModel(role);
                await Send.CreatedAtAsync<GetRoleByIdEndpoint>(
                    routeValues: new { id = role.Id.Value },
                    responseBody: response,
                    cancellation: ct
                );
            },
            Left: async error =>
            {
                await Send.NoContentAsync();
            });
    }
}