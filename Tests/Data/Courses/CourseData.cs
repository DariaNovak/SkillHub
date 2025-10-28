using Domain.Courses;
using Domain.Users;

namespace Tests.Data.Courses
{
    public static class CourseData
    {
        public static Course FirstCourse(UserId authorId) =>
            Course.New(
                CourseId.New(),
                "Introduction to C#",
                "Learn the basics of C# programming language.",
                authorId
            );

        public static Course SecondCourse(UserId authorId) =>
            Course.New(
                CourseId.New(),
                "ASP.NET Core Fundamentals",
                "Deep dive into building web applications with ASP.NET Core.",
                authorId
            );

        public static Course ThirdCourse(UserId authorId) =>
            Course.New(
                CourseId.New(),
                "Entity Framework Core",
                "Learn how to work with databases using EF Core.",
                authorId
            );

        public static Course WithCustomData(
            CourseId id,
            string title,
            string description,
            UserId authorId)
            => Course.New(
                id,
                title,
                description,
                authorId
            );
    }
}