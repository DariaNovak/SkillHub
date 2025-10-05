using Api.Dtos;
using Application.Users.Commands;
using Domain.Roles.Role;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

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
        var user = await _mediator.Send(command, ct);
        Response = UserDto.FromDomainModel(user);
    }
}