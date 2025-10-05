using Api.Dtos;
using Application.Courses.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Courses;

public class GetAllCourseEndpoint : EndpointWithoutRequest<List<CourseDto>>
{
    private readonly IMediator _mediator;

    public GetAllCourseEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/courses");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllCoursesQuery();
        var courses = await _mediator.Send(query, ct);
        Response = courses.Select(CourseDto.FromDomainModel).ToList();
    }
}