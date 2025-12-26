using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.ToTable("users");

        builder.Property(u => u.Id)
               .HasColumnName("id")
               .HasConversion(id => id.Value, value => new EntityId<User>(value))
               .ValueGeneratedOnAdd();

        builder.Property(u => u.Username)
               .HasColumnName("username")
               .HasMaxLength(50)
               .IsRequired();

        builder.HasIndex(u => u.Username, "users_username_key")
               .IsUnique();

        builder.Property(u => u.Email)
               .HasColumnName("email")
               .HasMaxLength(255);

        builder.HasIndex(u => u.Email, "users_email_key")
               .IsUnique();

        builder.Property(u => u.PasswordHash)
               .HasColumnName("password_hash")
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(u => u.IsAdmin)
               .HasColumnName("is_admin")
               .HasDefaultValue(false);

        builder.Property(u => u.Deleted)
               .HasColumnName("deleted")
               .HasDefaultValue(false);

        builder.HasOne(u => u.Profile)
               .WithOne()
               .HasForeignKey<Profile>(p => p.UserId);
    }
}