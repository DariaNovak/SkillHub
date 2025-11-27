using Api.Dtos;
using Application.UsersSkills.Queries;
using Domain.Users;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.UsersSkills;

public class GetUserSkillsByUserIdEndpoint : Endpoint<GetUserSkillsByUserIdDto, List<UserSkillDto>>
{
    private readonly IMediator _mediator;

    public GetUserSkillsByUserIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/userskills/{userId}/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserSkillsByUserIdDto req, CancellationToken ct)
    {
        var query = new GetUserSkillsByUserIdQuery
        {
            UserId = new UserId(req.UserId)
        };

        var userSkills = await _mediator.Send(query, ct);

        if (userSkills == null || !userSkills.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var dtos = userSkills.Select(UserSkillDto.FromDomainModel).ToList();
        await Send.OkAsync(dtos, ct);
    }
}
