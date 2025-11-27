using Api.Dtos;
using Application.Lessons.Commands;
using FastEndpoints;
using MediatR;
using LanguageExt;
using Unit = LanguageExt.Unit;

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
        var command = new DeleteLessonCommand { LessonId = req.Id };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async _ => await Send.NoContentAsync(ct),
            Left: async _ => await Send.NotFoundAsync(ct)
        );
    }
}
