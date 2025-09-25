using Application.Users.Queries;
using Domain.Users;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Users;

public class GetUsersEndpoint : EndpointWithoutRequest<List<User>>
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
        var users = await _mediator.Send(new GetAllUsersQuery(), ct);
        Response = users.ToList();
    }
}