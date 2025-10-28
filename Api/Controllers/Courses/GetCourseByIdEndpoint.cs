using Api.Dtos;
using Application.Courses.Queries;
using Domain.Courses;
using FastEndpoints;
using LanguageExt;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Api.Controllers.Courses;

public class GetCourseByIdEndpoint : EndpointWithoutRequest<CourseDto>
{
    private readonly IMediator _mediator;

    public GetCourseByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/courses/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<CourseId>("id");

        var query = new GetCourseByIdQuery(id);

        var command = await _mediator.Send(query, ct);

        await command.Match(
            Some: async course => Send.OkAsync(CourseDto.FromDomainModel(course)),
            None: async () => Send.NotFoundAsync(ct)
            );
    }
}
