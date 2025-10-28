using Domain.Courses;
using Domain.Lessons;

namespace Api.Dtos;

public record LessonDto(
    Guid Id,
    string Title,
    string Content,
    CourseId CourseId,
    int Order)
{
    public static LessonDto FromDomainModel(Lesson lesson)
        => new(
            lesson.Id,
            lesson.Title,
            lesson.Content,
            lesson.CourseId,
            lesson.Order);
}

public record CreateLessonDto(
    string Title,
    string Content,
    CourseId CourseId,
    int Order);

public record UpdateLessonDto(
    Guid Id,
    string Title,
    string Content, 
    int Order);

public record DeleteLessonDto(Guid Id);

public record GetLessonByIdDto(Guid Id);