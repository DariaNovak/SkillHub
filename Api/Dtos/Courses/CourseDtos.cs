using Domain.Courses;

namespace Api.Dtos;

public record CourseDto(
    CourseId Id,
    string Title,
    string Description,
    Guid AuthorId,
    DateTime CreatedAt)
{
    public static CourseDto FromDomainModel(Course course)
        => new(
            course.Id,
            course.Title,
            course.Description,
            course.AuthorId,
            course.CreatedAt);
}

public record CreateCourseDto(
    string Title,
    string Description,
    Guid AuthorId);

public record UpdateCourseDto(
    Guid Id,
    string Title,
    string Description);

public record DeleteCourseDto(Guid Id);

public record GetCourseByIdDto(Guid Id);