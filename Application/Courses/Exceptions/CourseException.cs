using Domain.Courses;

namespace Application.Courses.Exceptions
{
    public abstract class CourseException(CourseId courseId, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public CourseId CourseId { get; } = courseId;
    }

    public class CourseAlreadyExistsException(CourseId courseId)
        : CourseException(courseId, $"Course already exists under id {courseId}");

    public class CourseNotFoundException(CourseId courseId)
        : CourseException(courseId, $"Course not found under id {courseId}");

    public class UnhandledCourseException(CourseId courseId, Exception? innerException = null)
        : CourseException(courseId, "Unexpected error occurred", innerException);
}
