using Application.Common.Interfaces.Queries;
using Domain.Courses;
using MediatR;

namespace Application.Courses.Queries;

public record GetAllCoursesQuery() : IRequest<IReadOnlyList<Course>>;

public class GetAllCoursesQueryHandler(
    ICourseQueries courseRepository) : IRequestHandler<GetAllCoursesQuery, IReadOnlyList<Course>>
{
    public async Task<IReadOnlyList<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await courseRepository.GetAllAsync(cancellationToken);
    }
}