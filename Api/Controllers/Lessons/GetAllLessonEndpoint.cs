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
        var query = new GetAllLessonsQuery();
        var lessons = await _mediator.Send(query, ct);

        if (lessons == null || !lessons.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var dtos = lessons.Select(LessonDto.FromDomainModel).ToList();
        await Send.OkAsync(dtos, ct);
    }
}
