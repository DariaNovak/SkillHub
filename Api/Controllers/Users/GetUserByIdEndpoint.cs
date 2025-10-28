using Api.Dtos;
using Application.Users.Queries;
using Domain.Users;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Users;

public class GetUserByIdEndpoint : Endpoint<GetUserByIdDto, UserDto>
{
    private readonly IMediator _mediator;

    public GetUserByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/users/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserByIdDto req, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(new UserId(req.Id));
        var command = await _mediator.Send(query, ct);
        await command.Match(
            Some: async user => Send.OkAsync(UserDto.FromDomainModel(user)),
            None: async () => Send.NotFoundAsync(ct)
            );
    }
}