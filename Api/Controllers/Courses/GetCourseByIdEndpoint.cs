using Api.Dtos;
using Application.Courses.Queries;
using Domain.Courses;
using FastEndpoints;
using MediatR;

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
        var idGuid = Route<Guid>("id");
        var courseId = new CourseId(idGuid);

        var query = new GetCourseByIdQuery(courseId);

        var result = await _mediator.Send(query, ct);

        await result.Match(
            Some: async course => await Send.OkAsync(CourseDto.FromDomainModel(course), ct),
            None: async () => await Send.NotFoundAsync(ct)
        );
    }
}