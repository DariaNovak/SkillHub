using Api.Dtos;
using Application.Skills.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Skills;

public class UpdateSkillEndpoint : Endpoint<UpdateSkillDto>
{
    private readonly IMediator _mediator;

    public UpdateSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/skills/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateSkillDto req, CancellationToken ct)
    {
        var command = new UpdateSkillsCommand
        {
            Id = req.Id,
            Name = req.Name
        };

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}