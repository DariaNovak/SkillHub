using Application.Common.Interfaces.Queries;
using Domain.Lessons;
using MediatR;

namespace Application.Lessons.Queries;

public record GetLessonByIdQuery(Guid LessonId) : IRequest<Lesson?>;

public class GetLessonByIdQueryHandler(
    ILessonQueries lessonRepository) : IRequestHandler<GetLessonByIdQuery, Lesson?>
{
    public async Task<Lesson?> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        return await lessonRepository.GetByIdAsync(request.LessonId, cancellationToken);
    }
}