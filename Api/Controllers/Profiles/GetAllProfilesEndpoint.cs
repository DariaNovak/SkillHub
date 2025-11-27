using Api.Dtos;
using Application.Profiles.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Profiles;

public class GetAllProfilesEndpoint : EndpointWithoutRequest<List<ProfileDto>>
{
    private readonly IMediator _mediator;

    public GetAllProfilesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/profiles");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllProfilesQuery();
        var profiles = await _mediator.Send(query, ct);
        var dtos = profiles.Select(ProfileDto.FromDomainModel).ToList();
        Response = dtos;
    }
}
