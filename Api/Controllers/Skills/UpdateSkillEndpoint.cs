using Api.Dtos;
using Application.Skills.Commands;
using Application.Skills.Exceptions;
using Domain.Skills;
using FastEndpoints;
using Infrastructure.Persistence;
using MediatR;

namespace Api.Controllers.Skills;

public class UpdateSkillEndpoint : Endpoint<UpdateSkillDto, SkillDto>
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext context;

    public UpdateSkillEndpoint(IMediator mediator, ApplicationDbContext context)
    {
        this.context = context;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/skills/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateSkillDto req, CancellationToken ct)
    {
        var command = new UpdateSkillCommand
        {
            SkillId = new SkillId(req.Id),
            Name = req.Name
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async skill =>
            {
                var response = SkillDto.FromDomainModel(skill);
                await Send.NoContentAsync(ct);
            },
            Left: async ex =>
            {
                switch (ex)
                {
                    case SkillNotFoundException:
                        await Send.NotFoundAsync(ct);
                        break;

                    case SkillAlreadyExistsException:
                        await Send.NotFoundAsync(ct);
                        break;

                    case UnhandledSkillException:
                        await Send.NotFoundAsync(ct);
                        break;

                    default:
                        await Send.NotFoundAsync(ct);
                        break;
                }
            }
        );
    }
}