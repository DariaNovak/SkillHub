using Api.Dtos;
using Application.Profiles.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Profiles;

public class DeleteProfileEndpoint : Endpoint<DeleteProfileDto>
{
    private readonly IMediator _mediator;

    public DeleteProfileEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/profiles/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteProfileDto req, CancellationToken ct)
    {
        var command = new DeleteProfileCommand { ProfileId = req.Id };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async profile => Send.NoContentAsync(ct),
            Left: async error => Send.NotFoundAsync(ct)
        );
    }
}
