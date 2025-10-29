using Api.Dtos;
using Api.Endpoints.Users;
using Application.Users.Commands;
using Domain.Roles;
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
            RoleId = new RoleId(req.RoleId),
            JoinDate = req.JoinDate
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async user =>
            {
                var response = UserDto.FromDomainModel(user);
                await Send.CreatedAtAsync<GetUserByIdEndpoint>(
                    routeValues: new { id = user.Id.Value },
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