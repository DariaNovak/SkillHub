using Application.Common.Interfaces.Queries;
using Domain.Lessons;
using LanguageExt;
using MediatR;

namespace Application.Lessons.Queries;

public record GetLessonByIdQuery(LessonId Id) : IRequest<Option<Lesson>>;

public class GetLessonByIdQueryHandler(
    ILessonQueries lessonQueries) : IRequestHandler<GetLessonByIdQuery, Option<Lesson>>
{
    public async Task<Option<Lesson>> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        var option = await lessonQueries.GetByIdAsync(request.Id, cancellationToken);
        return option.Match(
            Some: lesson => lesson,
            None: () => Option<Lesson>.None
        );
    }
}
