using Application.Common.Interfaces.Queries;
using Domain.Lessons;
using LanguageExt;
using MediatR;

namespace Application.Lessons.Queries;

public record GetAllLessonsQuery() : IRequest<IReadOnlyList<Lesson>>;

public class GetAllLessonsQueryHandler(
    ILessonQueries lessonQueries)
    : IRequestHandler<GetAllLessonsQuery, IReadOnlyList<Lesson>>
{
    public async Task<IReadOnlyList<Lesson>> Handle(GetAllLessonsQuery request, CancellationToken cancellationToken)
    {
        var option = await lessonQueries.GetAllAsync(cancellationToken);

        return option.Match(
            Some: lessons => lessons,
            None: () => Array.Empty<Lesson>()
        );
    }
}
