using Api.Dtos;
using Application.Users.Commands;
using Application.Users.Exceptions;
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
           Right: user => Send.OkAsync(UserDto.FromDomainModel(user)),     // 200 OK
            Left: ex =>
            {
                switch (ex)
                {
                    case UserNotFoundException: Send.NotFoundAsync(); break; // 404
                    case UserAlreadyExistException: Send.ErrorsAsync(); break; // 409
                    default: Send.NotFoundAsync(); break;          // 400
                }
            }
        );
    }
}