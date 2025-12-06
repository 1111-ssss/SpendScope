using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

public class AppVersionConfiguration : IEntityTypeConfiguration<AppVersion>
{
    public void Configure(EntityTypeBuilder<AppVersion> builder)
    {
        builder.Property(v => v.UploadedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(v => v.UploadedBy)
               .OnDelete(DeleteBehavior.SetNull);
    }
}