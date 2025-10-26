using Api.Dtos;
using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Users;

public class UpdateUserEndpoint : Endpoint<UpdateUserDto, UserDto>
{
    private readonly IMediator _mediator;

    public UpdateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserDto req, CancellationToken ct)
    {
        var command = new UpdateUserCommand
        {
            UserId = req.Id,
            Name = req.Name,
            Email = req.Email,
            PasswordHash = req.PasswordHash,
            RoleId = req.RoleId,
            JoinDate = req.JoinDate
        };

        var result = await _mediator.Send(command, ct);

        result.Match(
            Right: user => Response = UserDto.FromDomainModel(user),
            Left: ex => ThrowError(ex.Message)
        );
    }
}