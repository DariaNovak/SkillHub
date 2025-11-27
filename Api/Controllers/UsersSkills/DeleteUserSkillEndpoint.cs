using Api.Dtos;
using Application.UsersSkills.Commands;
using Domain.Users;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.UsersSkills;

public class DeleteUserSkillEndpoint : Endpoint<DeleteUserSkillDto>
{
    private readonly IMediator _mediator;

    public DeleteUserSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/userskills/{userId}/skills/{skillId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserSkillDto req, CancellationToken ct)
    {
        var command = new DeleteUserSkillCommand
        {
            UserId = new UserId(req.UserId),
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
