using Api.Dtos;
using Application.Profiles.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Profiles;

public class GetProfileByUserIdEndpoint : Endpoint<GetProfileByIdDto, ProfileDto>
{
    private readonly IMediator _mediator;

    public GetProfileByUserIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/profiles/user/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProfileByIdDto req, CancellationToken ct)
    {
        var query = new GetProfileByUserIdQuery { UserId = req.Id };
        var result = await _mediator.Send(query, ct);

        await result.Match(
            Some: profile => Send.OkAsync(ProfileDto.FromDomainModel(profile), ct),
            None: () => Send.NotFoundAsync(ct)
        );
    }
}
