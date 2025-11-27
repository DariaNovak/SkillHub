using Api.Dtos;
using Application.CoursesSkills.Commands;
using Domain.Courses;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.CoursesSkills;

public class DeleteCourseSkillEndpoint : Endpoint<DeleteCourseSkillDto>
{
    private readonly IMediator _mediator;

    public DeleteCourseSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/courseskills/{courseId}/skills/{skillId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCourseSkillDto req, CancellationToken ct)
    {
        var command = new DeleteCourseSkillCommand
        {
            CourseId = new CourseId(req.CourseId),
            SkillId = new SkillId(req.SkillId)
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async success =>
            {
                await Send.NoContentAsync(ct);
            },
            Left: async error =>
            {
                await Send.NotFoundAsync(ct);
            });
    }
}
