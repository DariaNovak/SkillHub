using Api.Dtos;
using Application.Profiles.Commands;
using Domain.Users;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Profiles;

public class CreateProfileEndpoint : Endpoint<CreateProfileDto, ProfileDto>
{
    private readonly IMediator _mediator;

    public CreateProfileEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/profiles");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProfileDto req, CancellationToken ct)
    {
        var command = new CreateProfileCommand
        {
            UserId = new UserId(req.UserId),
            Bio = req.Bio,
            PhoneNumber = req.PhoneNumber,
            Location = req.Location,
            Website = req.Website
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async profile =>
            {
                var response = ProfileDto.FromDomainModel(profile);
                await Send.CreatedAtAsync<GetProfileByIdEndpoint>(
                    routeValues: new { id = profile.Id.Value },
                    responseBody: response,
                    cancellation: ct
                );
            },
            Left: async error =>
            {
                ThrowError(error.Message, 400);
            });
    }
}
