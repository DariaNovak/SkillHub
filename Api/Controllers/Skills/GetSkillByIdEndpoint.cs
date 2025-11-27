using Api.Dtos;
using Application.Skills.Queries;
using Domain.Skills;
using FastEndpoints;
using LanguageExt;
using MediatR;

namespace Api.Controllers.Skills;

public class GetSkillByIdEndpoint : EndpointWithoutRequest<SkillDto>
{
    private readonly IMediator _mediator;

    public GetSkillByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/skills/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var idGuid = Route<Guid>("id");
        var skillId = new SkillId(idGuid);

        var query = new GetSkillByIdQuery(skillId);

        var option = await _mediator.Send(query, ct);

        await option.Match(
            Some: async skill => await Send.OkAsync(SkillDto.FromDomainModel(skill), ct),
            None: async () => await Send.NotFoundAsync(ct)
        );
    }
}