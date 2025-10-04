using Domain.UsersSkills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
    {
        public void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.HasKey(x => new { x.UserId, x.SkillId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserSkills)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Skill)
                .WithMany(x => x.UserSkills)
                .HasForeignKey(x => x.SkillId);
        }
    }
}
