using Domain.Courses;
using Domain.Users;

namespace Api.Dtos;

public record CourseDto(
    CourseId Id,
    string Title,
    string Description,
    UserId AuthorId,
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
    UserId AuthorId);

public record UpdateCourseDto(
    CourseId Id,
    string Title,
    string Description,
    UserId AuthorId);

public record DeleteCourseDto(Guid Id);

public record GetCourseByIdDto(Guid Id);
