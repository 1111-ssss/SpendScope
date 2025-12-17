using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.HasKey(a => a.Id);
        builder.ToTable("achievements");

        builder.Property(a => a.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
               .HasColumnName("name")
               .HasMaxLength(100)
               .IsRequired();

        builder.HasIndex(a => a.Name, "achievements_name_key")
               .IsUnique();

        builder.Property(a => a.Description)
               .HasColumnName("description")
               .HasMaxLength(200);

        builder.Property(a => a.IconUrl)
               .HasColumnName("icon_url")
               .HasMaxLength(255);
    }
}