using Api.Dtos;
using Application.Users.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Users;

public class GetUsersEndpoint : EndpointWithoutRequest<List<UserDto>>
{
    private readonly IMediator _mediator;

    public GetUsersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query, ct);
        var dtos = users.Select(UserDto.FromDomainModel).ToList();
        Response = dtos;
    }
}