using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using Domain.Users;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Courses.Commands;

public record UpdateCourseCommand : IRequest<Either<CourseException, Course>>
{
    public required CourseId CourseId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required UserId AuthorId { get; init; }
}

public class UpdateCourseCommandHandler(
    ICourseQueries courseQueries,
    ICourseRepository courseRepository)
    : IRequestHandler<UpdateCourseCommand, Either<CourseException, Course>>
{
    public async Task<Either<CourseException, Course>> Handle(
        UpdateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await courseQueries.GetByIdAsync(request.CourseId, cancellationToken);

        return await course.MatchAsync(
            c => UpdateEntity(request, c, cancellationToken),
            () => new CourseNotFoundException(request.CourseId));
    }

    private async Task<Either<CourseException, Course>> UpdateEntity(
        UpdateCourseCommand request,
        Course course,
        CancellationToken cancellationToken)
    {
        try
        {
            course.UpdateInfo(request.Title, request.Description);

            await courseRepository.UpdateAsync(course, cancellationToken);
            return course;
        }
        catch (Exception exception)
        {
            return new UnhandledCourseException(course.Id, exception);
        }
    }
}
