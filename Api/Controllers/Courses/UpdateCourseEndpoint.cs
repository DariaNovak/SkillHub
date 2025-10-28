using Api.Dtos;
using Application.Courses.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Courses;

public class UpdateCourseEndpoint : Endpoint<UpdateCourseDto>
{
    private readonly IMediator _mediator;

    public UpdateCourseEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/courses/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCourseDto req, CancellationToken ct)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = req.Id,
            Title = req.Title,
            Description = req.Description,
            AuthorId = req.AuthorId
        };

        await _mediator.Send(command, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}
