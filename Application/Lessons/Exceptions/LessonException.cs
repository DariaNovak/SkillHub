using Domain.Courses;
using Domain.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Lessons.Exceptions
{
    public abstract class LessonException(LessonId lessonId, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public LessonId LessonId { get; } = lessonId;
    }

    public class LessonAlreadyExistsException(LessonId lessonId)
        : LessonException(lessonId, $"Lesson already exists under id {lessonId}");

    public class LessonNotFoundException(LessonId lessonId)
        : LessonException(lessonId, $"Lesson not found under id {lessonId}");

    public class UnhandledLessonException(LessonId lessonId, Exception? innerException = null)
        : LessonException(lessonId, "Unexpected error occurred", innerException);
}
