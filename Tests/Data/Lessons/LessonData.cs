using Domain.Courses;
using Domain.Lessons;

namespace Tests.Data.Lessons
{
    public static class LessonData
    {
        public static Lesson FirstLesson(CourseId courseId) =>
            Lesson.New(
                "Introduction to Variables",
                "Learn about variables and data types in programming.",
                courseId,
                1
            );

        public static Lesson SecondLesson(CourseId courseId) =>
            Lesson.New(
                "Control Flow Statements",
                "Understanding if-else statements, loops, and switch cases.",
                courseId,
                2
            );

        public static Lesson ThirdLesson(CourseId courseId) =>
            Lesson.New(
                "Functions and Methods",
                "Learn how to create and use functions in your code.",
                courseId,
                3
            );

        public static Lesson WithCustomData(
            string title,
            string content,
            CourseId courseId,
            int order)
            => Lesson.New(
                title,
                content,
                courseId,
                order
            );
    }
}