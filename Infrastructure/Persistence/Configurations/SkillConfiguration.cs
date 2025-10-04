using Domain.CoursesSkills;
using Domain.Skills;
using Domain.UsersSkills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();


            // Many-to-Many: Skill <-> User 
            builder.HasMany(typeof(UserSkill), "UserSkills")
                .WithOne("Skill")
                .HasForeignKey("SkillId");

            // Many-to-Many: Skill <-> Course 
             builder.HasMany(typeof(CourseSkill), "CourseSkills")
                .WithOne("Skill")
                .HasForeignKey("SkillId");
        }
    }
}
