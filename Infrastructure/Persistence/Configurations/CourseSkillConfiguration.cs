using Domain.CoursesSkills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CourseSkillConfiguration : IEntityTypeConfiguration<CourseSkill>
    {
        public void Configure(EntityTypeBuilder<CourseSkill> builder)
        {
            builder.HasKey(x => new { x.CourseId, x.SkillId });

            builder.HasOne(x => x.Course)
                .WithMany(x => x.CourseSkills)
                .HasForeignKey(x => x.CourseId);

            builder.HasOne(x => x.Skill)
                .WithMany(x => x.CourseSkills)
                .HasForeignKey(x => x.SkillId);
        }
    }
}
