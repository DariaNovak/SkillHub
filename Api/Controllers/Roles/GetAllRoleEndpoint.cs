using Api.Dtos;
using Application.Roles.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Roles;

public class GetAllRoleEndpoint : EndpointWithoutRequest<List<RoleDto>>
{
    private readonly IMediator _mediator;

    public GetAllRoleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/roles");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllRoleQuery();
        var roles = await _mediator.Send(query, ct);

        if (roles == null || !roles.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var dtos = roles.Select(RoleDto.FromDomainModel).ToList();
        await Send.OkAsync(dtos, ct);
    }
}