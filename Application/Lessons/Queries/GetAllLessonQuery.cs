using Application.Common.Interfaces.Queries;
using Domain.Lessons;
using MediatR;

namespace Application.Lessons.Queries;

public record GetAllLessonQuery() : IRequest<IReadOnlyList<Lesson>>;

public class GetAllLessonQueryHandler(
    ILessonQueries lessonRepository) : IRequestHandler<GetAllLessonQuery, IReadOnlyList<Lesson>>
{
    public async Task<IReadOnlyList<Lesson>> Handle(GetAllLessonQuery request, CancellationToken cancellationToken)
    {
        return await lessonRepository.GetAllAsync(cancellationToken);
    }
}