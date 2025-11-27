using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
    {
        public void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.HasKey(x => new { x.UserId, x.SkillId });

            builder.Property(x => x.Id)
                .HasConversion(x => x, x => x)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasConversion(x => x.Value, x => new UserId(x))
                .IsRequired();

            builder.Property(x => x.SkillId)
                .HasConversion(x => x.Value, x => new SkillId(x))
                .IsRequired();

            builder.Property(x => x.ProficiencyLevel)
                .IsRequired();

            builder.Property(x => x.AddedDate)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserSkills)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany(x => x.UserSkills)
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
