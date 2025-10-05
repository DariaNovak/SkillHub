using Api.Dtos;
using Application.Lessons.Queries;
using FastEndpoints;
using MediatR;

namespace Api.Controllers.Lessons;

public class GetAllLessonEndpoint : EndpointWithoutRequest<List<LessonDto>>
{
    private readonly IMediator _mediator;

    public GetAllLessonEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/lessons");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var lessons = await _mediator.Send(new GetAllLessonQuery(), ct);
        Response = lessons.Select(LessonDto.FromDomainModel).ToList(); ;
    }
}