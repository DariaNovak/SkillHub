using FastEndpoints;
using MediatR;
using Application.Lessons.Commands;

namespace Api.Controllers.Lessons;

public class DeleteLessonEndpoint : Endpoint<DeleteLessonCommand>
{
    private readonly IMediator _mediator;

    public DeleteLessonEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/lessons/{LessonId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteLessonCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        Response = StatusCodes.Status204NoContent;
    }
}