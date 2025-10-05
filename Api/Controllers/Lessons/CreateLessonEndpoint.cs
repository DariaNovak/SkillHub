using Api.Dtos;
using Application.Lessons.Commands;
using FastEndpoints;
using MediatR;

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
            Title = req.Title,
            Content = req.Content,
            CourseId = req.CourseId,
            Order = req.Order
        };
        var lesson = await _mediator.Send(command, ct);
        Response = LessonDto.FromDomainModel(lesson);
    }
}