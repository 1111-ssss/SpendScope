using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Entities;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> entity)
    {
        entity.HasKey(e => e.UserId).HasName("profiles_pkey");

        entity.ToTable("profiles");

        entity.Property(e => e.UserId)
            .ValueGeneratedNever()
            .HasColumnName("user_id");
        entity.Property(e => e.AvatarUrl)
            .HasMaxLength(255)
            .HasColumnName("avatar_url");
        entity.Property(e => e.Bio)
            .HasMaxLength(400)
            .HasColumnName("bio");
        entity.Property(e => e.DisplayName)
            .HasMaxLength(20)
            .HasColumnName("display_name");
        entity.Property(e => e.LastOnline)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("last_online");

        entity.HasOne(d => d.User).WithOne(p => p.Profile)
            .HasForeignKey<Profile>(d => d.UserId)
            .HasConstraintName("profiles_user_id_fkey");
    }
}