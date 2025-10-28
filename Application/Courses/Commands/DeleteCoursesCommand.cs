using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Courses.Commands;

public record DeleteCourseCommand : IRequest<Either<CourseException, Unit>>
{
    public required Guid CourseId { get; init; }
}

public class DeleteCourseCommandHandler(
    ICourseQueries courseQueries,
    ICourseRepository courseRepository)
    : IRequestHandler<DeleteCourseCommand, Either<CourseException, Unit>>
{
    public async Task<Either<CourseException, Unit>> Handle(
        DeleteCourseCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var course = await courseQueries.GetByIdAsync(courseId, cancellationToken);

        return await course.MatchAsync(
            c => DeleteEntity(c.Id, cancellationToken),
            () => new CourseNotFoundException(courseId.Value));
    }

    private async Task<Either<CourseException, Unit>> DeleteEntity(
        CourseId id,
        CancellationToken cancellationToken)
    {
        try
        {
            await courseRepository.DeleteAsync(id, cancellationToken);
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new UnhandledCourseException(id.Value, exception);
        }
    }
}