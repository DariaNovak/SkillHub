using Api.Dtos;
using Application.Roles.Commands;
using Application.Roles.Exceptions;
using Domain.Roles;
using FastEndpoints;
using Infrastructure.Persistence;
using MediatR;

namespace Api.Controllers.Roles;

public class UpdateRoleEndpoint : Endpoint<UpdateRoleDto, RoleDto>
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext context;

    public UpdateRoleEndpoint(IMediator mediator, ApplicationDbContext context)
    {
        this.context = context;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/roles/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateRoleDto req, CancellationToken ct)
    {
        var command = new UpdateRoleCommand
        {
            RoleId = new RoleId(req.Id),
            Name = req.Name
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async role =>
            {
                var response = RoleDto.FromDomainModel(role);
                await Send.NoContentAsync(ct);
            },
            Left: async ex =>
            {
                switch (ex)
                {
                    case RoleNotFoundException:
                        await Send.NotFoundAsync(ct);
                        break;

                    case RoleAlreadyExistsException:
                        await Send.NotFoundAsync(ct);
                        break;

                    case UnhandledRoleException:
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