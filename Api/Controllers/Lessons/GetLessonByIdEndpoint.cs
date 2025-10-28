using Api.Dtos;
using Application.Lessons.Queries;
using Domain.Lessons;
using FastEndpoints;
using LanguageExt;
using MediatR;

namespace Api.Controllers.Lessons;

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
        var id = Route<LessonId>("id"); 

        var query = new GetLessonByIdQuery(id);

        var option = await _mediator.Send(query, ct);

        await option.Match(
            Some: async lesson => await Send.OkAsync(LessonDto.FromDomainModel(lesson), ct),
            None: async () => await Send.NotFoundAsync(ct)
        );
    }
}
