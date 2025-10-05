using Api.Dtos;
using Application.Skills.Commands;
using FastEndpoints;
using MediatR;

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

    public override async Task HandleAsync(DeleteSkillDto  req, CancellationToken ct)
    {
        var command = new DeleteSkillsCommand(req.Id);

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}