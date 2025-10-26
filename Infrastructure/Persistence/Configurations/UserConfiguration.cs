using Domain.Users;
using Infrastructure.Persistence.Converters;
using LanguageExt.ClassInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.Name)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.JoinDate)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.RoleId)
           .IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: User -> Skills
        builder.HasMany(x => x.UserSkills)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

    }
}