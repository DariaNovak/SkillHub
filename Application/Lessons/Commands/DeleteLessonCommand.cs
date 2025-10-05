using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Lessons.Commands;

public record DeleteLessonCommand(Guid LessonId) : IRequest;

public class DeleteLessonCommandHandler(
    ILessonRepository lessonRepository) : IRequestHandler<DeleteLessonCommand>
{
    public async Task Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        await lessonRepository.DeleteAsync(request.LessonId, cancellationToken);
    }
}