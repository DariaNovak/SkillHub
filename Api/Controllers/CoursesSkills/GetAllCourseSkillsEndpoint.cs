using Api.Dtos;
using Application.CoursesSkills.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.CoursesSkills;

public class GetAllCourseSkillsEndpoint : EndpointWithoutRequest<List<CourseSkillDto>>
{
    private readonly IMediator _mediator;

    public GetAllCourseSkillsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/courseskills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllCourseSkillsQuery();
        var courseSkills = await _mediator.Send(query, ct);

        if (courseSkills == null || !courseSkills.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var dtos = courseSkills.Select(CourseSkillDto.FromDomainModel).ToList();
        await Send.OkAsync(dtos, ct);
    }
}
