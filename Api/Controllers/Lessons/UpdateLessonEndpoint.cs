using Api.Dtos;
using Application.Lessons.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Lessons;

public class UpdateLessonEndpoint : Endpoint<UpdateLessonDto>
{
    private readonly IMediator _mediator;

    public UpdateLessonEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/lessons/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateLessonDto req, CancellationToken ct)
    {
        var command = new UpdateLessonCommand(
            req.Id,
            req.Title,
            req.Content,
            req.Order
        );

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}