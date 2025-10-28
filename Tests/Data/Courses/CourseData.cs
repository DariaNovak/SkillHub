using Domain.Courses;
using Domain.Users;

namespace Tests.Data.Courses
{
    public static class CourseData
    {
        public static Course FirstCourse() =>
            Course.New(
                "Introduction to C#",
                "Learn the basics of C# programming language.",
                UserId.New()
            );

        public static Course SecondCourse() =>
            Course.New(
                "ASP.NET Core Fundamentals",
                "Deep dive into building web applications with ASP.NET Core.",
                UserId.New()
            );

        public static Course ThirdCourse() =>
            Course.New(
                "Entity Framework Core",
                "Learn how to work with databases using EF Core.",
                UserId.New()
            );

        public static Course WithCustomData(
            string title,
            string description,
            UserId authorId)
            => Course.New(
                title,
                description,
                authorId
            );
    }
}
