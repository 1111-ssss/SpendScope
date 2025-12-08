using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Entities;

public class AppVersionConfiguration : IEntityTypeConfiguration<AppVersion>
{
    public void Configure(EntityTypeBuilder<AppVersion> entity)
    {
        entity.HasKey(e => e.Id).HasName("appversions_pkey");

        entity.ToTable("app_versions");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("nextval('appversions_id_seq'::regclass)")
            .HasColumnName("id");
        entity.Property(e => e.Branch)
            .HasMaxLength(20)
            .HasColumnName("branch");
        entity.Property(e => e.Build).HasColumnName("build");
        entity.Property(e => e.Changelog)
            .HasMaxLength(250)
            .HasColumnName("changelog");
        entity.Property(e => e.UploadedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("uploaded_at");
        entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");

        entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.AppVersions)
            .HasForeignKey(d => d.UploadedBy)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("appversions_uploaded_by_fkey");
    }
}