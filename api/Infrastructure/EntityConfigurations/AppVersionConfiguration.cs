using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppVersionConfiguration : IEntityTypeConfiguration<AppVersion>
{
    public void Configure(EntityTypeBuilder<AppVersion> builder)
    {
        builder.HasKey(av => av.Id);
        builder.ToTable("app_versions");

        builder.Property(av => av.Id)
               .HasColumnName("id")
               .HasDefaultValueSql("nextval('appversions_id_seq'::regclass)")
               .ValueGeneratedOnAdd();

        builder.Property(av => av.Branch)
               .HasColumnName("branch")
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(av => av.Build)
               .HasColumnName("build")
               .IsRequired();

        builder.Property(av => av.Changelog)
               .HasColumnName("changelog")
               .HasMaxLength(250);

        builder.Property(av => av.UploadedAt)
               .HasColumnName("uploaded_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(av => av.UploadedBy)
               .HasColumnName("uploaded_by")
               .HasConversion(
                   id => id.HasValue ? id.Value.Value : (int?)null,
                   value => value.HasValue ? new EntityId<User>(value.Value) : (EntityId<User>?)null)
               .IsRequired(false);

        builder.HasIndex(av => av.UploadedBy);
        builder.HasIndex(av => av.UploadedAt);
    }
}