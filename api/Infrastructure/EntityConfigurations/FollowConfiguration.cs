using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Entities;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> entity)
    {
        entity.HasKey(e => new { e.FollowerId, e.FollowedId }).HasName("follows_pkey");

        entity.ToTable("follows");

        entity.HasIndex(e => e.FollowedId, "idx_follows_followed");

        entity.HasIndex(e => e.FollowerId, "idx_follows_follower");

        entity.Property(e => e.FollowerId).HasColumnName("follower_id");
        entity.Property(e => e.FollowedId).HasColumnName("followed_id");
        entity.Property(e => e.FollowedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("followed_at");

        entity.HasOne(d => d.Followed).WithMany(p => p.FollowFolloweds)
            .HasForeignKey(d => d.FollowedId)
            .HasConstraintName("follows_followed_id_fkey");

        entity.HasOne(d => d.Follower).WithMany(p => p.FollowFollowers)
            .HasForeignKey(d => d.FollowerId)
            .HasConstraintName("follows_follower_id_fkey");
    }
}