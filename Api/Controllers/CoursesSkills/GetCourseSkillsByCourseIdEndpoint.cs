using Api.Dtos;
using Application.CoursesSkills.Queries;
using Domain.Courses;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.CoursesSkills;

public class GetCourseSkillsByCourseIdEndpoint : Endpoint<GetCourseSkillsByCourseIdDto, List<CourseSkillDto>>
{
    private readonly IMediator _mediator;

    public GetCourseSkillsByCourseIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/courseskills/{courseId}/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCourseSkillsByCourseIdDto req, CancellationToken ct)
    {
        var query = new GetCourseSkillsByCourseIdQuery
        {
            CourseId = new CourseId(req.CourseId)
        };

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
