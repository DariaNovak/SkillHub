using Api.Dtos;
using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Users;

public class CreateUserEndpoint : Endpoint<CreateUserDto, UserDto>
{
    private readonly IMediator _mediator;

    public CreateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserDto req, CancellationToken ct)
    {
        var command = new CreateUserCommand
        {
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