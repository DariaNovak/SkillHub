using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Roles.Role;
using Domain.Users;
using Domain.Skills;
using Domain.Courses;
using Domain.Lessons;
using Domain.Profiles;
using Domain.UsersSkills;
using Domain.CoursesSkills;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext dbContext)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Check if database is already seeded
        if (dbContext.Roles.Any())
        {
            return;
        }

        // Seed Roles
        var adminRole = Role.New("Admin");
        var userRole = Role.New("User");
        var moderatorRole = Role.New("Moderator");

        await dbContext.Roles.AddRangeAsync(adminRole, userRole, moderatorRole);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded roles");

        // Seed Users
        var users = new[]
        {
            User.New("Alice Johnson", "alice.johnson@example.com", "hashed_password_1", adminRole.Id, DateTime.UtcNow),
            User.New("Bob Smith", "bob.smith@example.com", "hashed_password_2", userRole.Id, DateTime.UtcNow),
            User.New("Charlie Brown", "charlie.brown@example.com", "hashed_password_3", userRole.Id, DateTime.UtcNow),
            User.New("Diana Prince", "diana.prince@example.com", "hashed_password_4", moderatorRole.Id, DateTime.UtcNow),
            User.New("Eve Adams", "eve.adams@example.com", "hashed_password_5", userRole.Id, DateTime.UtcNow)
        };

        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded users");

        // Seed Skills
        var skills = new[]
        {
            Skill.New("C# Programming"),
            Skill.New("ASP.NET Core"),
            Skill.New("Entity Framework"),
            Skill.New("JavaScript"),
            Skill.New("React"),
            Skill.New("Angular"),
            Skill.New("SQL"),
            Skill.New("MongoDB"),
            Skill.New("Docker"),
            Skill.New("Kubernetes")
        };

        await dbContext.Skills.AddRangeAsync(skills);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded skills");

        // Seed UserSkills
        var userSkills = new[]
        {
            UserSkill.New(users[0].Id, skills[0].Id, 5), // Alice - C# (Expert)
            UserSkill.New(users[0].Id, skills[1].Id, 5), // Alice - ASP.NET Core (Expert)
            UserSkill.New(users[0].Id, skills[2].Id, 4), // Alice - Entity Framework (Advanced)
            UserSkill.New(users[1].Id, skills[3].Id, 4), // Bob - JavaScript (Advanced)
            UserSkill.New(users[1].Id, skills[4].Id, 3), // Bob - React (Intermediate)
            UserSkill.New(users[2].Id, skills[0].Id, 3), // Charlie - C# (Intermediate)
            UserSkill.New(users[2].Id, skills[6].Id, 4), // Charlie - SQL (Advanced)
            UserSkill.New(users[3].Id, skills[8].Id, 5), // Diana - Docker (Expert)
            UserSkill.New(users[3].Id, skills[9].Id, 4), // Diana - Kubernetes (Advanced)
            UserSkill.New(users[4].Id, skills[5].Id, 3)  // Eve - Angular (Intermediate)
        };

        await dbContext.UserSkills.AddRangeAsync(userSkills);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded user skills");

        // Seed Profiles
        var profiles = new[]
        {
            Profile.New(
                users[0].Id,
                "Senior Software Engineer with 10+ years of experience in .NET development.",
                "+1-555-0101",
                "New York, USA",
                "https://alice-johnson.dev"
            ),
            Profile.New(
                users[1].Id,
                "Full-stack developer passionate about modern web technologies.",
                "+1-555-0102",
                "San Francisco, USA",
                "https://bobsmith.com"
            ),
            Profile.New(
                users[2].Id,
                "Database enthusiast and backend developer.",
                "+1-555-0103",
                "Chicago, USA",
                "https://charliebrown.tech"
            ),
            Profile.New(
                users[3].Id,
                "DevOps engineer specializing in container orchestration.",
                "+44-20-5550104",
                "London, UK",
                "https://dianaprince.io"
            )
        };

        await dbContext.Profiles.AddRangeAsync(profiles);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded profiles");

        // Seed Courses
        var courses = new[]
        {
            Course.New(
                CourseId.New(),
                "Introduction to C# Programming",
                "Learn the fundamentals of C# programming language, including syntax, data types, control structures, and object-oriented programming concepts.",
                users[0].Id
            ),
            Course.New(
                CourseId.New(),
                "ASP.NET Core Web Development",
                "Build modern web applications using ASP.NET Core. Learn about MVC, Web API, authentication, and deployment.",
                users[0].Id
            ),
            Course.New(
                CourseId.New(),
                "Modern JavaScript Essentials",
                "Master JavaScript ES6+ features, async programming, and modern development practices.",
                users[1].Id
            ),
            Course.New(
                CourseId.New(),
                "React for Beginners",
                "Learn React fundamentals, hooks, state management, and how to build interactive user interfaces.",
                users[1].Id
            ),
            Course.New(
                CourseId.New(),
                "Database Design with SQL",
                "Comprehensive guide to database design, normalization, and SQL queries.",
                users[2].Id
            ),
            Course.New(
                CourseId.New(),
                "Docker and Containerization",
                "Learn how to containerize applications using Docker and manage containers in production.",
                users[3].Id
            )
        };

        await dbContext.Courses.AddRangeAsync(courses);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded courses");

        // Seed CourseSkills
        var courseSkills = new[]
        {
            CourseSkill.New(courses[0], skills[0]), // C# Course -> C# Skill
            CourseSkill.New(courses[1], skills[0]), // ASP.NET Course -> C# Skill
            CourseSkill.New(courses[1], skills[1]), // ASP.NET Course -> ASP.NET Core Skill
            CourseSkill.New(courses[1], skills[2]), // ASP.NET Course -> Entity Framework Skill
            CourseSkill.New(courses[2], skills[3]), // JavaScript Course -> JavaScript Skill
            CourseSkill.New(courses[3], skills[3]), // React Course -> JavaScript Skill
            CourseSkill.New(courses[3], skills[4]), // React Course -> React Skill
            CourseSkill.New(courses[4], skills[6]), // SQL Course -> SQL Skill
            CourseSkill.New(courses[5], skills[8])  // Docker Course -> Docker Skill
        };

        await dbContext.CourseSkills.AddRangeAsync(courseSkills);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded course skills");

        // Seed Lessons
        var lessons = new List<Lesson>();

        // Lessons for C# Course
        lessons.Add(Lesson.New(
            "Introduction to C#",
            "Overview of C# language, its history, and basic syntax. Setting up development environment.",
            courses[0].Id,
            1
        ));
        lessons.Add(Lesson.New(
            "Variables and Data Types",
            "Understanding value types, reference types, and how to work with variables in C#.",
            courses[0].Id,
            2
        ));
        lessons.Add(Lesson.New(
            "Control Flow",
            "Learn about if-else statements, switch cases, loops, and flow control in C#.",
            courses[0].Id,
            3
        ));

        // Lessons for ASP.NET Course
        lessons.Add(Lesson.New(
            "Getting Started with ASP.NET Core",
            "Introduction to ASP.NET Core framework and creating your first web application.",
            courses[1].Id,
            1
        ));
        lessons.Add(Lesson.New(
            "MVC Pattern",
            "Understanding the Model-View-Controller pattern in ASP.NET Core applications.",
            courses[1].Id,
            2
        ));
        lessons.Add(Lesson.New(
            "Building REST APIs",
            "Learn how to create RESTful APIs using ASP.NET Core Web API.",
            courses[1].Id,
            3
        ));

        // Lessons for JavaScript Course
        lessons.Add(Lesson.New(
            "JavaScript Basics",
            "Introduction to JavaScript syntax, variables, and basic operations.",
            courses[2].Id,
            1
        ));
        lessons.Add(Lesson.New(
            "ES6+ Features",
            "Modern JavaScript features including arrow functions, destructuring, and modules.",
            courses[2].Id,
            2
        ));
        lessons.Add(Lesson.New(
            "Asynchronous JavaScript",
            "Understanding promises, async/await, and handling asynchronous operations.",
            courses[2].Id,
            3
        ));

        // Lessons for React Course
        lessons.Add(Lesson.New(
            "Introduction to React",
            "What is React, JSX syntax, and creating your first component.",
            courses[3].Id,
            1
        ));
        lessons.Add(Lesson.New(
            "React Hooks",
            "Learn useState, useEffect, and other essential React hooks.",
            courses[3].Id,
            2
        ));

        // Lessons for SQL Course
        lessons.Add(Lesson.New(
            "Database Fundamentals",
            "Introduction to relational databases and SQL basics.",
            courses[4].Id,
            1
        ));
        lessons.Add(Lesson.New(
            "Writing SQL Queries",
            "Learn to write SELECT, INSERT, UPDATE, and DELETE statements.",
            courses[4].Id,
            2
        ));

        // Lessons for Docker Course
        lessons.Add(Lesson.New(
            "Introduction to Docker",
            "Understanding containers, images, and Docker basics.",
            courses[5].Id,
            1
        ));
        lessons.Add(Lesson.New(
            "Dockerfile and Images",
            "Learn how to create Docker images using Dockerfile.",
            courses[5].Id,
            2
        ));

        await dbContext.Lessons.AddRangeAsync(lessons);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeded lessons");
        logger.LogInformation("Database seeding completed successfully");
    }
}
