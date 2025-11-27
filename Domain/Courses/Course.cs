using Domain.CoursesSkills;
using Domain.Lessons;
using Domain.Users;

namespace Domain.Courses
{
    public class Course
    {
        public CourseId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserId AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Author { get; private set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<CourseSkill> CourseSkills { get; set; }

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

        public static Course New(CourseId id, string title, string description, UserId authorId)
        {
            return new Course(
                id,
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