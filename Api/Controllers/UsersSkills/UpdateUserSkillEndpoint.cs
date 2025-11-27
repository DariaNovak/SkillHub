using Api.Dtos;
using Application.UsersSkills.Commands;
using Domain.Users;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.UsersSkills;

public class UpdateUserSkillEndpoint : Endpoint<UpdateUserSkillDto, UserSkillDto>
{
    private readonly IMediator _mediator;

    public UpdateUserSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/userskills/{userId}/skills/{skillId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserSkillDto req, CancellationToken ct)
    {
        var command = new UpdateUserSkillCommand
        {
            UserId = new UserId(req.UserId),
            SkillId = new SkillId(req.SkillId),
            ProficiencyLevel = req.ProficiencyLevel
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async userSkill =>
            {
                var response = UserSkillDto.FromDomainModel(userSkill);
                await Send.OkAsync(response, ct);
            },
            Left: async error =>
            {
                await Send.NotFoundAsync(ct);
            });
    }
}
