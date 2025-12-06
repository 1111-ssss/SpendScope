using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.UserId);

        builder.Property(p => p.LastOnline)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne<User>()
               .WithOne()
               .HasForeignKey<Profile>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}