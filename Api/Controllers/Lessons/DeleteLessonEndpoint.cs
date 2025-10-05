using Api.Dtos;
using Application.Lessons.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Lessons;

public class DeleteLessonEndpoint : Endpoint<DeleteLessonDto>
{
    private readonly IMediator _mediator;

    public DeleteLessonEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/lessons/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteLessonDto req, CancellationToken ct)
    {
        var command = new DeleteLessonCommand(req.Id);

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}