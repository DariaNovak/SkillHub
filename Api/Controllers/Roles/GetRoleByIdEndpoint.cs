using Api.Dtos;
using Application.Roles.Queries;
using Domain.Roles;
using FastEndpoints;
using LanguageExt;
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
        var idGuid = Route<Guid>("id");
        var roleId = new RoleId(idGuid);

        var query = new GetRoleByIdQuery(roleId);

        var option = await _mediator.Send(query, ct);

        await option.Match(
            Some: async role => await Send.OkAsync(RoleDto.FromDomainModel(role), ct),
            None: async () => await Send.NotFoundAsync(ct)
        );
    }
}