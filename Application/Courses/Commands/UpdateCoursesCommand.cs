using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using MediatR;

namespace Application.Courses.Commands;

public record UpdateCourseCommand : IRequest<Course>
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public class UpdateCourseCommandHandler(
    ICourseQueries courseQueries,
    ICourseRepository courseRepository) : IRequestHandler<UpdateCourseCommand, Course>
{
    public async Task<Course> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseQueries.GetByIdAsync(request.Id, cancellationToken);
        if (course is null)
            throw new KeyNotFoundException("Course not found.");

        course.UpdateInfo(request.Title, request.Description);
        await courseRepository.UpdateAsync(course, cancellationToken);

        return course;
    }
}