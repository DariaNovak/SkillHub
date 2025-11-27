using Api.Dtos;
using Application.Profiles.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Profiles;

public class GetProfileByIdEndpoint : Endpoint<GetProfileByIdDto, ProfileDto>
{
    private readonly IMediator _mediator;

    public GetProfileByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/profiles/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProfileByIdDto req, CancellationToken ct)
    {
        var query = new GetProfileByIdQuery { ProfileId = req.Id };
        var result = await _mediator.Send(query, ct);

        await result.Match(
            Some: profile => Send.OkAsync(ProfileDto.FromDomainModel(profile), ct),
            None: () => Send.NotFoundAsync(ct)
        );
    }
}
