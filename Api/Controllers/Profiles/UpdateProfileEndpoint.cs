using Api.Dtos;
using Application.Profiles.Commands;
using Application.Profiles.Exceptions;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Profiles;

public class UpdateProfileEndpoint : Endpoint<UpdateProfileDto, ProfileDto>
{
    private readonly IMediator _mediator;

    public UpdateProfileEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/profiles/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateProfileDto req, CancellationToken ct)
    {
        var command = new UpdateProfileCommand
        {
            ProfileId = req.Id,
            Bio = req.Bio,
            PhoneNumber = req.PhoneNumber,
            Location = req.Location,
            Website = req.Website
        };

        var result = await _mediator.Send(command, ct);

        result.Match(
            Right: profile => Send.NoContentAsync(ct),
            Left: ex =>
            {
                switch (ex)
                {
                    case ProfileNotFoundException: Send.NotFoundAsync(ct); break;
                    default: ThrowError(ex.Message, 400); break;
                }
            }
        );
    }
}
