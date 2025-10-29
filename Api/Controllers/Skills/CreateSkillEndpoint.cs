using Api.Dtos;
using Application.Skills.Commands;
using Domain.Skills;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Skills;

public class CreateSkillEndpoint : Endpoint<CreateSkillDto, SkillDto>
{
    private readonly IMediator _mediator;

    public CreateSkillEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/skills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateSkillDto req, CancellationToken ct)
    {
        var command = new CreateSkillCommand
        {
            Id = SkillId.New(),
            Name = req.Name
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async skill =>
            {
                var response = SkillDto.FromDomainModel(skill);
                await Send.CreatedAtAsync<GetSkillByIdEndpoint>(
                    routeValues: new { id = skill.Id.Value },
                    responseBody: response,
                    cancellation: ct
                );
            },
            Left: async error =>
            {
                await Send.NoContentAsync();
            });
    }
}