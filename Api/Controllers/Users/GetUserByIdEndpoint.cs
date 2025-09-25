using Api.Dtos;
using Application.Users.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Users;

public class GetUserByIdEndpoint : EndpointWithoutRequest<UserDto>
{
    private readonly IMediator _mediator;

    public GetUserByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query, ct);
        if (user is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        Response = UserDto.FromDomainModel(user);
    }
}
