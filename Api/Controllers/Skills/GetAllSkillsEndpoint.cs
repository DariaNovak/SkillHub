using FastEndpoints;
using MediatR;
using Application.Skills.Queries;
using Domain.Skills;
using System.Collections.Generic;

namespace Api.Controllers.Skills;

public class GetAllSkillsEndpoint : Endpoint<GetAllSkillsQuery, IEnumerable<Skill>>
{
    private readonly IMediator _mediator;

    public GetAllSkillsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllSkillsQuery req, CancellationToken ct)
    {
        var skills = await _mediator.Send(req, ct);
        Response = skills;
    }
}