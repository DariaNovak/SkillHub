using Api.Dtos;
using Application.Lessons.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Endpoints.Lessons;

public class GetLessonByIdEndpoint : EndpointWithoutRequest<LessonDto>
{
    private readonly IMediator _mediator;

    public GetLessonByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/lessons/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var query = new GetLessonByIdQuery(id);
        var lesson = await _mediator.Send(query, ct);
        if (lesson is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        Response = LessonDto.FromDomainModel(lesson);
    }
}