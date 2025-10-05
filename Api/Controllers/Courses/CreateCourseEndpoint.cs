using Api.Dtos;
using Application.Courses.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Courses;

public class CreateCourseEndpoint : Endpoint<CreateCourseDto, CourseDto>
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

    public override async Task HandleAsync(CreateCourseDto req, CancellationToken ct)
    {
        var command = new CreateCourseCommand
        {
            Title = req.Title,
            Description = req.Description,
            AuthorId = req.AuthorId
        };
        var course = await _mediator.Send(command, ct);
        Response = CourseDto.FromDomainModel(course);
    }
}