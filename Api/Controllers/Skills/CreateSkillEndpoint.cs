using Api.Dtos;
using Application.Skills.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Skills;

public class CreateSkillEndpoint : Endpoint<CreateSkillDto, SkillDto>
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

    public override async Task HandleAsync(CreateSkillDto req, CancellationToken ct)
    {
        var command = new CreateSkillsCommand
        {
            Name = req.Name
        };
        var skill = await _mediator.Send(command, ct);
        Response = SkillDto.FromDomainModel(skill);
    }
}