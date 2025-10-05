using Api.Dtos;
using Application.Skills.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Skills;

public class GetAllSkillsEndpoint : EndpointWithoutRequest<List<SkillDto>>
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

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllSkillsQuery();
        var skills = await _mediator.Send(query, ct);
        Response = skills.Select(SkillDto.FromDomainModel).ToList();
    }
}