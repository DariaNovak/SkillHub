using Application.Common.Interfaces.Queries;
using Domain.Courses;
using MediatR;

namespace Application.Courses.Queries;

public record GetCourseByIdQuery(Guid CourseId) : IRequest<Course?>;

public class GetCourseByIdQueryHandler(
    ICourseQueries courseRepository) : IRequestHandler<GetCourseByIdQuery, Course?>
{
    public async Task<Course?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        return await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
    }
}