using Api.Dtos;
using Application.Courses.Commands;
using FastEndpoints;
using MediatR;
using LanguageExt;

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
        var command = new DeleteCourseCommand { CourseId = req.Id };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async _ => await Send.NoContentAsync(ct),
            Left: async _ => await Send.NotFoundAsync(ct)
        );
    }
}
