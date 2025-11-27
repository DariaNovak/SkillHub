using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Lessons.Exceptions;
using Domain.Lessons;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Lessons.Commands;

public record DeleteLessonCommand : IRequest<Either<LessonException, Unit>>
{
    public required Guid LessonId { get; init; }
}

public class DeleteLessonCommandHandler(
    ILessonQueries lessonQueries,
    ILessonRepository lessonRepository)
    : IRequestHandler<DeleteLessonCommand, Either<LessonException, Unit>>
{
    public async Task<Either<LessonException, Unit>> Handle(
        DeleteLessonCommand request,
        CancellationToken cancellationToken)
    {
        var lessonId = new LessonId(request.LessonId);
        var lesson = await lessonQueries.GetByIdAsync(lessonId, cancellationToken);

        return await lesson.MatchAsync(
            l => DeleteEntity(l.Id, cancellationToken),
            () => new LessonNotFoundException(lessonId));
    }

    private async Task<Either<LessonException, Unit>> DeleteEntity(
        LessonId id,
        CancellationToken cancellationToken)
    {
        try
        {
            await lessonRepository.DeleteAsync(id, cancellationToken);
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new UnhandledLessonException(id, exception);
        }
    }
}
