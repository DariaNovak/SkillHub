using Api.Dtos;
using Application.Skills.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Skills;

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
        var id = Route<Guid>("id");
        var query = new GetSkillByIdQuery(id);
        var skill = await _mediator.Send(query, ct);
        if (skill is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        Response = SkillDto.FromDomainModel(skill);
    }
}