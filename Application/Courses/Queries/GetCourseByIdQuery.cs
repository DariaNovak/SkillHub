using Application.Common.Interfaces.Queries;
using Domain.Courses;
using LanguageExt;
using MediatR;

namespace Application.Courses.Queries;

public record GetCourseByIdQuery(CourseId Id) : IRequest<Option<Course>>;

public class GetCourseByIdQueryHandler(
    ICourseQueries courseQueries)
    : IRequestHandler<GetCourseByIdQuery, Option<Course>>
{
    public async Task<Option<Course>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var option = await courseQueries.GetByIdAsync(request.Id, cancellationToken);

        return option.Match(
            Some: course => course,
            None: () => Option<Course>.None
        );
    }
}
