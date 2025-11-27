using Api.Dtos;
using Application.UsersSkills.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.UsersSkills;

public class GetAllUserSkillsEndpoint : EndpointWithoutRequest<List<UserSkillDto>>
{
    private readonly IMediator _mediator;

    public GetAllUserSkillsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/userskills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllUserSkillsQuery();
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
