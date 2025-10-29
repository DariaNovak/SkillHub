using Api.Dtos;
using Application.Lessons.Commands;
using Application.Lessons.Exceptions;
using Domain.Lessons;
using FastEndpoints;
using Infrastructure.Persistence;
using MediatR;

namespace Api.Controllers.Lessons;

public class UpdateLessonEndpoint : Endpoint<UpdateLessonDto, LessonDto>
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext context;

    public UpdateLessonEndpoint(IMediator mediator, ApplicationDbContext context)
    {
        this.context = context;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/lessons/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateLessonDto req, CancellationToken ct)
    {
        var command = new UpdateLessonCommand
        {
            LessonId = new LessonId(req.Id),
            Title = req.Title,
            Content = req.Content,
            Order = req.Order
        };

        var result = await _mediator.Send(command, ct);

        await result.Match(
            Right: async lesson =>
            {
                var response = LessonDto.FromDomainModel(lesson);
                await Send.NoContentAsync(ct);
            },
            Left: async ex =>
            {
                switch (ex)
                {
                    case LessonNotFoundException:
                        await Send.NotFoundAsync(ct);
                        break;

                    case LessonAlreadyExistsException:
                        await Send.NotFoundAsync(ct);
                        break;

                    case UnhandledLessonException:
                        await Send.NotFoundAsync(ct);
                        break;

                    default:
                        await Send.NotFoundAsync(ct);
                        break;
                }
            }
        );
    }
}