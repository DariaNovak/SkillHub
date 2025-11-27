using Api.Dtos;
using Application.UsersSkills.Commands;
using Domain.Users;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.UsersSkills;

public class CreateUserSkillEndpoint : Endpoint<CreateUserSkillDto, UserSkillDto>
{
    private readonly IMediator _mediator;

    public CreateUserSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/userskills/{userId}/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserSkillDto req, CancellationToken ct)
    {
        var command = new CreateUserSkillCommand
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
                await Send.CreatedAtAsync<GetUserSkillByIdEndpoint>(
                    routeValues: new { userId = userSkill.UserId.Value, skillId = userSkill.SkillId.Value },
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
