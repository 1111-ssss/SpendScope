using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id).HasName("users_pkey");

        entity.ToTable("users");

        entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

        entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        entity.Property(e => e.Deleted)
            .HasDefaultValue(false)
            .HasColumnName("deleted");
        entity.Property(e => e.Email)
            .HasMaxLength(255)
            .HasColumnName("email");
        entity.Property(e => e.IsAdmin)
            .HasDefaultValue(false)
            .HasColumnName("is_admin");
        entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
        entity.Property(e => e.Username)
            .HasMaxLength(50)
            .HasColumnName("username");
    }
}