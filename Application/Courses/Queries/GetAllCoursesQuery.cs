using Application.Common.Interfaces.Queries;
using Domain.Courses;
using LanguageExt;
using MediatR;

namespace Application.Courses.Queries;

public record GetAllCoursesQuery() : IRequest<IReadOnlyList<Course>>;

public class GetAllCoursesQueryHandler(
    ICourseQueries courseQueries)
    : IRequestHandler<GetAllCoursesQuery, IReadOnlyList<Course>>
{
    public async Task<IReadOnlyList<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var option = await courseQueries.GetAllAsync(cancellationToken);

        return option.Match(
            Some: courses => courses,
            None: () => Array.Empty<Course>()
        );
    }
}
