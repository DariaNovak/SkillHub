using Api.Dtos;
using Application.Courses.Commands;
using Domain.Courses;
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
            Id = CourseId.New(),
            Title = req.Title,
            Description = req.Description,
            AuthorId = req.AuthorId
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async course =>
            {
                var response = CourseDto.FromDomainModel(course);
                await Send.CreatedAtAsync<GetCourseByIdEndpoint>(
                    routeValues: new { id = course.Id.Value },
                    responseBody: response,
                    cancellation: ct
                );
            },
            Left: async error =>
            {
                await Send.NotFoundAsync(ct);
            });
    }
}