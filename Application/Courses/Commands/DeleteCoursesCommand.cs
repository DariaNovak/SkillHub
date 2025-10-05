using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Courses.Commands;

public record DeleteCourseCommand(Guid CourseId) : IRequest;

public class DeleteCourseCommandHandler(
    ICourseRepository courseRepository) : IRequestHandler<DeleteCourseCommand>
{
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        await courseRepository.DeleteAsync(request.CourseId, cancellationToken);
    }
}