using Domain.Profiles;
using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ProfileId(x));

        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();

        builder.Property(x => x.Bio)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnType("varchar(50)");

        builder.Property(x => x.Location)
            .HasColumnType("varchar(255)");

        builder.Property(x => x.Website)
            .HasColumnType("varchar(500)");

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired(false);

        // One-to-One: Profile -> User
        builder.HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .HasForeignKey<Profile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}
