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
        var roles = await _mediator.Send(new GetAllRoleQuery(), ct);
        Response = roles.Select(RoleDto.FromDomainModel).ToList(); ;
    }
}