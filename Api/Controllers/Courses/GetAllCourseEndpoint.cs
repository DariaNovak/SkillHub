using Application.Courses.Queries;
using Domain.Courses;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Courses;

public class GetAllCourseEndpoint : EndpointWithoutRequest<List<Course>>
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
        var courses = await _mediator.Send(new GetAllCoursesQuery(), ct);
        Response = courses.ToList();
    }
}