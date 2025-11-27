using Api.Dtos;
using Application.CoursesSkills.Commands;
using Domain.Courses;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.CoursesSkills;

public class CreateCourseSkillEndpoint : Endpoint<CreateCourseSkillDto, CourseSkillDto>
{
    private readonly IMediator _mediator;

    public CreateCourseSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/courseskills/{courseId}/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCourseSkillDto req, CancellationToken ct)
    {
        var command = new CreateCourseSkillCommand
        {
            CourseId = new CourseId(req.CourseId),
            SkillId = new SkillId(req.SkillId)
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async courseSkill =>
            {
                var response = CourseSkillDto.FromDomainModel(courseSkill);
                await Send.CreatedAtAsync<GetCourseSkillByIdEndpoint>(
                    routeValues: new { courseId = courseSkill.CourseId.Value, skillId = courseSkill.SkillId.Value },
                    responseBody: response,
                    cancellation: ct
                );
            },
            Left: async error =>
            {
                await Send.NoContentAsync(ct);
            });
    }
}
