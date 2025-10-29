using Api.Dtos;
using Application.Users.Commands;
using Application.Users.Exceptions;
using Domain.Roles;
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
            RoleId = new RoleId(req.RoleId),
            JoinDate = req.JoinDate
        };

        var result = await _mediator.Send(command, ct);

        result.Match(
           Right: user => Send.OkAsync(UserDto.FromDomainModel(user)),     
            Left: ex =>
            {
                switch (ex)
                {
                    case UserNotFoundException: Send.NotFoundAsync(); break; 
                    case UserAlreadyExistException: Send.ErrorsAsync(); break; 
                    default: Send.NotFoundAsync(); break;         
                }
            }
        );
    }
}