using Api.Dtos;
using Application.Roles.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Roles;

public class GetRoleByIdEndpoint : EndpointWithoutRequest<RoleDto>
{
    private readonly IMediator _mediator;

    public GetRoleByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/roles/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var query = new GetRoleByIdQuery(id);
        var role = await _mediator.Send(query, ct);
        if (role is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        Response = RoleDto.FromDomainModel(role);
    }
}