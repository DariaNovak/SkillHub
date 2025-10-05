using FastEndpoints;
using MediatR;
using Application.Courses.Commands;
using Domain.Courses;

namespace Api.Controllers.Courses;

public class CreateCourseEndpoint : Endpoint<CreateCourseCommand, Course>
{
    private readonly IMediator _mediator;

    public CreateCourseEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/courses");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCourseCommand req, CancellationToken ct)
    {
        var course = await _mediator.Send(req, ct);
        Response = course;
    }
}