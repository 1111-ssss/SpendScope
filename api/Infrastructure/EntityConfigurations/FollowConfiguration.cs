using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.HasKey(f => new { f.FollowerId, f.FollowedId });
        builder.ToTable("follows");

        builder.Property(f => f.FollowerId)
               .HasColumnName("follower_id")
               .HasConversion(id => id.Value, value => new EntityId<User>(value));

        builder.Property(f => f.FollowedId)
               .HasColumnName("followed_id")
               .HasConversion(id => id.Value, value => new EntityId<User>(value));

        builder.Property(f => f.FollowedAt)
               .HasColumnName("followed_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP")
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.HasIndex(f => f.FollowedId, "idx_follows_followed");
        builder.HasIndex(f => f.FollowerId, "idx_follows_follower");
    }
}