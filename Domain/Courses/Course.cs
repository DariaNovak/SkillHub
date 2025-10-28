using Domain.Users;
using Domain.CoursesSkills;
using Domain.Lessons;

namespace Domain.Courses
{
    public class Course
    {
        public CourseId Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public UserId AuthorId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User Author { get; private set; }
        public ICollection<Lesson> Lessons { get; private set; }
        public ICollection<CourseSkill> CourseSkills { get; private set; }

        private Course(CourseId id, string title, string description, UserId authorId, DateTime createdAt)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorId = authorId;
            CreatedAt = createdAt;
            Lessons = new List<Lesson>();
            CourseSkills = new List<CourseSkill>();
        }

        public static Course New(string title, string description, UserId authorId)
        {
            return new Course(
                CourseId.New(),
                title,
                description,
                authorId,
                DateTime.UtcNow
            );
        }

        public void UpdateInfo(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
