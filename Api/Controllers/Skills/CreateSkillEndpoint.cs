using FastEndpoints;
using MediatR;
using Application.Skills.Commands;
using Api.Dtos;

namespace Api.Controllers.Skills;

public class CreateSkillEndpoint : Endpoint<CreateSkillsCommand, SkillDto>
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
        Response = SkillDto.FromDomainModel(skill);
    }
}