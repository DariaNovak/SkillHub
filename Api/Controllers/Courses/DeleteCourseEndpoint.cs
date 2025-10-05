using FastEndpoints;
using MediatR;
using Application.Courses.Commands;

namespace Api.Controllers.Courses;

public class DeleteCourseEndpoint : Endpoint<DeleteCourseCommand>
{
    private readonly IMediator _mediator;

    public DeleteCourseEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/courses/{CourseId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCourseCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        Response = StatusCodes.Status204NoContent;
    }
}