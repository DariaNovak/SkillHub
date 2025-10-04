using Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.AuthorId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // One-to-Many: Course -> Lessons
        builder.HasMany(x => x.Lessons)
            .WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId);

        // Many-to-Many: Course <-> Skill 
        builder.HasMany(x => x.CourseSkills)
            .WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId);
    }
}
