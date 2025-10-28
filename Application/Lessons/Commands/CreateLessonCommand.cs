using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using Domain.Lessons;
using MediatR;

namespace Application.Lessons.Commands;

public record CreateLessonCommand : IRequest<Lesson>
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required CourseId CourseId { get; init; }
    public required int Order { get; init; }
}

public class CreateLessonCommandHandler(
    ILessonRepository lessonRepository) : IRequestHandler<CreateLessonCommand, Lesson>
{
    public async Task<Lesson> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await lessonRepository.AddAsync(
            Lesson.New(request.Title, request.Content, request.CourseId, request.Order),
            cancellationToken);

        return lesson;
    }
}