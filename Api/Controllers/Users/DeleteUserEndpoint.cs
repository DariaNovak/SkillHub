using Api.Dtos;
using Application.Users.Commands;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Users;

public class DeleteUserEndpoint : Endpoint<DeleteUserDto>
{
    private readonly IMediator _mediator;

    public DeleteUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserDto req, CancellationToken ct)
    {
        var command = new DeleteUserCommand { UserId = req.Id };

        var result = await _mediator.Send(command, ct);

        
        await result.Match(
            Right: async user => Send.NoContentAsync(ct),  
            Left: async user => Send.NotFoundAsync(ct)      
        );
    }
}