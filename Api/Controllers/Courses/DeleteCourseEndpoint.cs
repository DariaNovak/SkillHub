using Api.Dtos;
using Application.Courses.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Courses;

public class DeleteCourseEndpoint : Endpoint<DeleteCourseDto>
{
    private readonly IMediator _mediator;

    public DeleteCourseEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/courses/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCourseDto req, CancellationToken ct)
    {
        var command = new DeleteCourseCommand(req.Id);

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}