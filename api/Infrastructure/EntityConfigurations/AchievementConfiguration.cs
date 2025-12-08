using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Entities;

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> entity)
    {
        entity.HasKey(e => e.Id).HasName("achievements_pkey");

        entity.ToTable("achievements");

        entity.HasIndex(e => e.Name, "achievements_name_key").IsUnique();

        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Description)
            .HasMaxLength(200)
            .HasColumnName("description");
        entity.Property(e => e.IconUrl)
            .HasMaxLength(255)
            .HasColumnName("icon_url");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
    }
}