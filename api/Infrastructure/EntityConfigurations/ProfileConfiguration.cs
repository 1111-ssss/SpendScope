using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.UserId);
        builder.ToTable("profiles");

        builder.Property(p => p.UserId)
               .HasColumnName("user_id")
               .HasConversion(id => id.Value, value => new EntityId<User>(value))
               .ValueGeneratedNever();

        builder.Property(p => p.DisplayName)
               .HasColumnName("display_name")
               .HasMaxLength(20);

        builder.Property(p => p.AvatarUrl)
               .HasColumnName("avatar_url")
               .HasMaxLength(255);

        builder.Property(p => p.Bio)
               .HasColumnName("bio")
               .HasMaxLength(400);

        builder.Property(p => p.LastOnline)
               .HasColumnName("last_online")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .HasColumnType("timestamp with time zone");

        builder.HasIndex(p => p.DisplayName);
    }
}