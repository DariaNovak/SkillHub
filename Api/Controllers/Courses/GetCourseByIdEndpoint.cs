using Api.Dtos;
using Application.Courses.Queries;
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
        var id = Route<Guid>("id");
        var query = new GetCourseByIdQuery(id);
        var course = await _mediator.Send(query, ct);
        if (course is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        Response = CourseDto.FromDomainModel(course);
    }
}