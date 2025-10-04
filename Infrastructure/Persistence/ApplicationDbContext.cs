using Domain.Courses;
using Domain.CoursesSkills;
using Domain.Lessons;
using Domain.Roles.Role;
using Domain.Skills;
using Domain.Users;
using Domain.UsersSkills;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<Skill> Skills { get; init; }
    public DbSet<UserSkill> UserSkills { get; init; }
    public DbSet<Course> Courses { get; init; }
    public DbSet<CourseSkill> CourseSkills { get; init; }
    public DbSet<Lesson> Lessons { get; init; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
