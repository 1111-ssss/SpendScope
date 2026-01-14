using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
       public void Configure(EntityTypeBuilder<RefreshToken> builder)
       {
              builder.HasKey(a => a.Id);
              builder.ToTable("refresh_tokens");

              builder.Property(a => a.Id)
                     .HasColumnName("id")
                     .HasConversion(id => id.Value, value => new EntityId<RefreshToken>(value))
                     .ValueGeneratedOnAdd();

              builder.Property(a => a.Token)
                     .HasColumnName("token")
                     .IsRequired();

              builder.Property(a => a.ExpiresAt)
                     .HasColumnName("expires_at")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP")
                     .HasColumnType("timestamp with time zone")
                     .IsRequired();

              builder.Property(a => a.CreatedAt)
                     .HasColumnName("created_at")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP")
                     .HasColumnType("timestamp with time zone")
                     .IsRequired();

              builder.Property(a => a.CreatedByIp)
                     .HasColumnName("created_by_ip")
                     .IsRequired();

              builder.Property(a => a.UserId)
                     .HasColumnName("user_id")
                     .HasConversion(id => id.Value, value => new EntityId<User>(value))
                     .IsRequired();
       }
}