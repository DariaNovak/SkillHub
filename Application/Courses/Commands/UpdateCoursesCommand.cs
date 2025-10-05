using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using MediatR;

namespace Application.Courses.Commands;

public record UpdateCourseCommand(
     Guid Id, 
     string Title, 
     string Description 
) : IRequest;

public class UpdateCourseCommandHandler(
    ICourseQueries courseQueries,
    ICourseRepository courseRepository) : IRequestHandler<UpdateCourseCommand>
{
    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseQueries.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
            throw new KeyNotFoundException("Course not found.");

        course.UpdateInfo(request.Title, request.Description);
        await courseRepository.UpdateAsync(course, cancellationToken);

    }
}