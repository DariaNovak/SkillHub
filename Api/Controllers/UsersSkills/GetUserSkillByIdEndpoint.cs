using Api.Dtos;
using Application.UsersSkills.Queries;
using Domain.Users;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.UsersSkills;

public class GetUserSkillByIdEndpoint : Endpoint<GetUserSkillByIdDto, UserSkillDto>
{
    private readonly IMediator _mediator;

    public GetUserSkillByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/userskills/{userId}/skills/{skillId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserSkillByIdDto req, CancellationToken ct)
    {
        var query = new GetUserSkillByIdQuery
        {
            UserId = new UserId(req.UserId),
            SkillId = new SkillId(req.SkillId)
        };

        var userSkill = await _mediator.Send(query, ct);

        await userSkill.Match(
            Some: async us =>
            {
                var dto = UserSkillDto.FromDomainModel(us);
                await Send.OkAsync(dto, ct);
            },
            None: async () =>
            {
                await Send.NotFoundAsync(ct);
            });
    }
}
