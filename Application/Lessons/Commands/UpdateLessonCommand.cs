using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Lessons.Exceptions;
using Domain.Lessons;
using LanguageExt;
using MediatR;

namespace Application.Lessons.Commands;

public record UpdateLessonCommand : IRequest<Either<LessonException, Lesson>>
{
    public required LessonId LessonId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required int Order { get; init; }
}

public class UpdateLessonCommandHandler(
    ILessonQueries lessonQueries,
    ILessonRepository lessonRepository)
    : IRequestHandler<UpdateLessonCommand, Either<LessonException, Lesson>>
{
    public async Task<Either<LessonException, Lesson>> Handle(
        UpdateLessonCommand request,
        CancellationToken cancellationToken)
    {
        var lesson = await lessonQueries.GetByIdAsync(request.LessonId, cancellationToken);

        return await lesson.MatchAsync(
            l => UpdateEntity(request, l, cancellationToken),
            () => new LessonNotFoundException(request.LessonId));
    }

    private async Task<Either<LessonException, Lesson>> UpdateEntity(
        UpdateLessonCommand request,
        Lesson lesson,
        CancellationToken cancellationToken)
    {
        try
        {
            lesson.UpdateInfo(request.Title, request.Content, request.Order);
            await lessonRepository.UpdateAsync(lesson, cancellationToken);
            return lesson;
        }
        catch (Exception exception)
        {
            return new UnhandledLessonException(lesson.Id, exception);
        }
    }
}
