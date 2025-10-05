using FastEndpoints;
using MediatR;
using Application.Courses.Commands;
using Api.Dtos;

namespace Api.Controllers.Courses;

public class CreateCourseEndpoint : Endpoint<CreateCourseCommand, CourseDto>
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
        Response = CourseDto.FromDomainModel(course);
    }
}