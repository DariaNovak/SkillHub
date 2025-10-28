using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Lessons.Exceptions;
using Domain.Courses;
using Domain.Lessons;
using LanguageExt;
using MediatR;

namespace Application.Lessons.Commands;

public record CreateLessonCommand : IRequest<Either<LessonException, Lesson>>
{
    public required LessonId Id { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required CourseId CourseId { get; init; }
    public required int Order { get; init; }
}

public class CreateLessonCommandHandler(
    ILessonQueries lessonQueries,
    ILessonRepository lessonRepository)
    : IRequestHandler<CreateLessonCommand, Either<LessonException, Lesson>>
{
    public async Task<Either<LessonException, Lesson>> Handle(
        CreateLessonCommand request,
        CancellationToken cancellationToken)
    {
        var existingLesson = await lessonQueries.GetByIdAsync(request.Id, cancellationToken);

        return await existingLesson.MatchAsync(
            l => new LessonAlreadyExistsException(request.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<LessonException, Lesson>> CreateEntity(
        CreateLessonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var lesson = await lessonRepository.AddAsync(
                Lesson.New(request.Title, request.Content, request.CourseId, request.Order),
                cancellationToken);

            return lesson;
        }
        catch (Exception exception)
        {
            return new UnhandledLessonException(request.Id, exception);
        }
    }
}
