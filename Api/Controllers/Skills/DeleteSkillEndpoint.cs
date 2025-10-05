using FastEndpoints;
using MediatR;
using Application.Skills.Commands;

namespace Api.Controllers.Skills;

public class DeleteSkillEndpoint : Endpoint<DeleteSkillsCommand>
{
    private readonly IMediator _mediator;

    public DeleteSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/skills/{SkillId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteSkillsCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        Response = StatusCodes.Status204NoContent;
    }
}