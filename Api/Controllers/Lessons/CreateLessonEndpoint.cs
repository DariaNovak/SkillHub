using Api.Dtos;
using Application.Lessons.Commands;
using FastEndpoints;
using MediatR;
using Domain.Lessons;

namespace Api.Controllers.Lessons;

public class CreateLessonEndpoint : Endpoint<CreateLessonDto, LessonDto>
{
    private readonly IMediator _mediator;

    public CreateLessonEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/lessons");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateLessonDto req, CancellationToken ct)
    {
        var command = new CreateLessonCommand
        {
            Id = LessonId.New(), 
            Title = req.Title,
            Content = req.Content,
            CourseId = req.CourseId,
            Order = req.Order
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async lesson =>
            {
                var response = LessonDto.FromDomainModel(lesson);
                await Send.CreatedAtAsync<GetLessonByIdEndpoint>(
                    routeValues: new { id = lesson.Id.Value },
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
