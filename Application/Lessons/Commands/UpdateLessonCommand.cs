using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Lessons;
using MediatR;

namespace Application.Lessons.Commands;

public record UpdateLessonCommand (
     Guid Id,
     string Title,
     string Content,
     int Order
) : IRequest;

public class UpdateLessonCommandHandler(
    ILessonQueries lessonQueries,
    ILessonRepository lessonRepository) : IRequestHandler<UpdateLessonCommand>
{
    public async Task Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await lessonQueries.GetByIdAsync(request.Id, cancellationToken);
        if (lesson == null)
            throw new KeyNotFoundException("Lesson not found.");

        lesson.UpdateInfo(request.Title, request.Content, request.Order);

        await lessonRepository.UpdateAsync(lesson, cancellationToken);
    }
}