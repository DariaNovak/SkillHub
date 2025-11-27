using Api.Dtos;
using Application.CoursesSkills.Queries;
using Domain.Courses;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.CoursesSkills;

public class GetCourseSkillByIdEndpoint : Endpoint<GetCourseSkillByIdDto, CourseSkillDto>
{
    private readonly IMediator _mediator;

    public GetCourseSkillByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/courseskills/{courseId}/skills/{skillId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCourseSkillByIdDto req, CancellationToken ct)
    {
        var query = new GetCourseSkillByIdQuery
        {
            CourseId = new CourseId(req.CourseId),
            SkillId = new SkillId(req.SkillId)
        };

        var courseSkill = await _mediator.Send(query, ct);

        await courseSkill.Match(
            Some: async cs =>
            {
                var dto = CourseSkillDto.FromDomainModel(cs);
                await Send.OkAsync(dto, ct);
            },
            None: async () =>
            {
                await Send.NotFoundAsync(ct);
            });
    }
}
