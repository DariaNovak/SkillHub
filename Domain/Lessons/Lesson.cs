using Domain.Courses;

namespace Domain.Lessons
{
    public class Lesson
    {
        public LessonId Id { get; set; }
        public string Title { get;  set; }
        public string Content { get; set; }
        public CourseId CourseId { get;  set; }
        public int Order { get;  set; }

        public Course Course { get;  set; }

        private Lesson(LessonId id, string title, string content, CourseId courseId, int order)
        {
            Id = id;
            Title = title;
            Content = content;
            CourseId = courseId;
            Order = order;
        }

        public static Lesson New(string title, string content, CourseId courseId, int order)
        {
           

            return new Lesson(LessonId.New(), title, content, courseId, order);
        }
        public void UpdateInfo(string title, string content, int order)
        {
            Title = title;
            Content = content;
            Order = order;
        }
    }
}
