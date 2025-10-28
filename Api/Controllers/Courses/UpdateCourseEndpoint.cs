using Api.Dtos;
using Application.Courses.Commands;
using Application.Courses.Exceptions;
using Domain.Courses;
using FastEndpoints;
using Infrastructure.Persistence;
using MediatR;

namespace Api.Controllers.Courses;

public class UpdateCourseEndpoint : Endpoint<UpdateCourseDto, CourseDto>
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext context;

    public UpdateCourseEndpoint(IMediator mediator, ApplicationDbContext context)
    {
        this.context = context;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/courses/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCourseDto req, CancellationToken ct)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = new CourseId(req.Id),
            Title = req.Title,
            Description = req.Description,
            AuthorId = req.AuthorId
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async course =>
            {
                var response = CourseDto.FromDomainModel(course);
                await Send.NoContentAsync();
            },
            Left: async ex =>
            {
                switch (ex)
                {
                    case CourseNotFoundException:
                        await Send.NotFoundAsync();
                        break;

                    case CourseAlreadyExistsException:
                        await Send.NotFoundAsync();
                        break;

                    case UnhandledCourseException:
                        await Send.NotFoundAsync();
                        break;

                    default:
                        await Send.NotFoundAsync();
                        break;
                }
            }
        );
    }
}