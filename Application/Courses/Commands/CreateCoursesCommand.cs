using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using LanguageExt;
using MediatR;

namespace Application.Courses.Commands;

public record CreateCourseCommand : IRequest<Either<CourseException, Course>>
{
    public required CourseId Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required Guid AuthorId { get; init; }
}

public class CreateCourseCommandHandler(
    ICourseQueries courseQueries,
    ICourseRepository courseRepository)
    : IRequestHandler<CreateCourseCommand, Either<CourseException, Course>>
{
    public async Task<Either<CourseException, Course>> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var existingCourse = await courseQueries.GetByIdAsync(request.Id, cancellationToken);

        return await existingCourse.MatchAsync(
            c => new CourseAlreadyExistsException(request.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<CourseException, Course>> CreateEntity(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseRepository.AddAsync(
                Course.New( request.Title, request.Description, request.AuthorId),
                cancellationToken);

            return course;
        }
        catch (Exception exception)
        {
            return new UnhandledCourseException(request.Id, exception);
        }
    }
}
