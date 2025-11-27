using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CourseSkillConfiguration : IEntityTypeConfiguration<CourseSkill>
    {
        public void Configure(EntityTypeBuilder<CourseSkill> builder)
        {
            builder.HasKey(x => new { x.CourseId, x.SkillId });

            builder.Property(x => x.CourseId)
                .HasConversion(x => x.Value, x => new CourseId(x))
                .IsRequired();

            builder.Property(x => x.SkillId)
                .HasConversion(x => x.Value, x => new SkillId(x))
                .IsRequired();

            builder.HasOne(x => x.Course)
                .WithMany(x => x.CourseSkills)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany(x => x.CourseSkills)
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
