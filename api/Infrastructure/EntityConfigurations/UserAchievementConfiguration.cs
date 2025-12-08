using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.Entities;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> entity)
    {
        entity.HasKey(e => new { e.UserId, e.AchievementId }).HasName("user_achievements_pkey");

        entity.ToTable("user_achievements");

        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
        entity.Property(e => e.UnlockedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("unlocked_at");

        entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements)
            .HasForeignKey(d => d.AchievementId)
            .HasConstraintName("user_achievements_achievement_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.UserAchievements)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("user_achievements_user_id_fkey");
    }
}