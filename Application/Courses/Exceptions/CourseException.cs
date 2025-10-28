using Domain.Courses;

namespace Application.Courses.Exceptions
{
    public abstract class CourseException(Guid courseId, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public Guid CourseId { get; } = courseId;
    }

    public class CourseAlreadyExistsException(Guid courseId)
        : CourseException(courseId, $"Course already exists under id {courseId}");

    public class CourseNotFoundException(Guid courseId)
        : CourseException(courseId, $"Course not found under id {courseId}");

    public class UnhandledCourseException(Guid courseId, Exception? innerException = null)
        : CourseException(courseId, "Unexpected error occurred", innerException);
}