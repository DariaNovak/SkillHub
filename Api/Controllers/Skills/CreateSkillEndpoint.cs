using FastEndpoints;
using MediatR;
using Application.Skills.Commands;
using Domain.Skills;

namespace Api.Controllers.Skills;

public class CreateSkillEndpoint : Endpoint<CreateSkillsCommand, Skill>
{
    private readonly IMediator _mediator;

    public CreateSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateSkillsCommand req, CancellationToken ct)
    {
        var skill = await _mediator.Send(req, ct);
        Response = skill;
    }
}