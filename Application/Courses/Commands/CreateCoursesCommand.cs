using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using MediatR;

namespace Application.Courses.Commands;

public record CreateCourseCommand : IRequest<Course>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required Guid AuthorId { get; init; }
}

public class CreateCourseCommandHandler(
    ICourseRepository courseRepository) : IRequestHandler<CreateCourseCommand, Course>
{
    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.AddAsync(
            Course.New(request.Title, request.Description, request.AuthorId),
            cancellationToken);

        return course;
    }
}