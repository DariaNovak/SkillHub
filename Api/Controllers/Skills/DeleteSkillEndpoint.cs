using Api.Dtos;
using Application.Skills.Commands;
using FastEndpoints;
using MediatR;
using LanguageExt;
using Unit = LanguageExt.Unit;

namespace Api.Controllers.Skills;

public class DeleteSkillEndpoint : Endpoint<DeleteSkillDto>
{
    private readonly IMediator _mediator;

    public DeleteSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/skills/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteSkillDto req, CancellationToken ct)
    {
        var command = new DeleteSkillCommand { SkillId = req.Id };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async _ => await Send.NoContentAsync(ct),
            Left: async _ => await Send.NotFoundAsync(ct)
        );
    }
}