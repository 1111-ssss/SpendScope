using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Deleted).HasDefaultValue(false);
        builder.Property(u => u.IsAdmin).HasDefaultValue(false);
        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}