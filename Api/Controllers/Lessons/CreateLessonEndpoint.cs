using FastEndpoints;
using MediatR;
using Application.Lessons.Commands;
using Domain.Lessons;

namespace Api.Controllers.Lessons;

public class CreateLessonEndpoint : Endpoint<CreateLessonCommand, Lesson>
{
    private readonly IMediator _mediator;

    public CreateLessonEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/lessons");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateLessonCommand req, CancellationToken ct)
    {
        var lesson = await _mediator.Send(req, ct);
        Response = lesson;
    }
}