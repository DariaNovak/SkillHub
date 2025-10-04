using Domain.Users;
using Domain.CoursesSkills;
using Domain.Lessons;

namespace Domain.Courses
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid AuthorId { get; private set; } // FK → User
        public DateTime CreatedAt { get; private set; }

        public User Author { get; private set; }
        public ICollection<Lesson> Lessons { get; private set; }
        public ICollection<CourseSkill> CourseSkills { get; private set; }

        private Course(Guid id, string title, string description, Guid authorId, DateTime createdAt)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorId = authorId;
            CreatedAt = createdAt;
            Lessons = new List<Lesson>();
            CourseSkills = new List<CourseSkill>();
        }

        public static Course New(string title, string description, User author)
        {
            return new Course(
                Guid.NewGuid(),
                title,
                description,
                author.Id,
                DateTime.Now
            );
        }

        public void UpdateInfo(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
