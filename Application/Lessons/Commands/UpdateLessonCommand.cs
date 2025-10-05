using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Lessons;
using MediatR;

namespace Application.Lessons.Commands;

public record UpdateLessonCommand : IRequest<Lesson>
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required int Order { get; init; }
}

public class UpdateLessonCommandHandler(
    ILessonQueries lessonQueries,
    ILessonRepository lessonRepository) : IRequestHandler<UpdateLessonCommand, Lesson>
{
    public async Task<Lesson> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await lessonQueries.GetByIdAsync(request.Id, cancellationToken);
        if (lesson is null)
            throw new KeyNotFoundException("Lesson not found.");

        lesson.UpdateInfo(request.Title, request.Content, request.Order);
        await lessonRepository.UpdateAsync(lesson, cancellationToken);

        return lesson;
    }
}