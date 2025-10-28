using Domain.Courses;

namespace Domain.Lessons
{
    public class Lesson
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public CourseId CourseId { get; private set; } // FK → Course
        public int Order { get; private set; }

        public Course Course { get; private set; }

        private Lesson(Guid id, string title, string content, CourseId courseId, int order)
        {
            Id = id;
            Title = title;
            Content = content;
            CourseId = courseId;
            Order = order;
        }

        public static Lesson New(string title, string content, CourseId courseId, int order)
        {
           

            return new Lesson(Guid.NewGuid(), title, content, courseId, order);
        }
        public void UpdateInfo(string title, string content, int order)
        {
            Title = title;
            Content = content;
            Order = order;
        }
    }
}
