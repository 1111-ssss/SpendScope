using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
       public void Configure(EntityTypeBuilder<UserAchievement> builder)
       {
              builder.HasKey(ua => new { ua.UserId, ua.AchievementId });
              builder.ToTable("user_achievements");

              builder.Property(ua => ua.UserId)
                     .HasColumnName("user_id")
                     .HasConversion(id => id.Value, value => new EntityId<User>(value));

              builder.Property(ua => ua.AchievementId)
                     .HasColumnName("achievement_id")
                     .HasConversion(id => id.Value, value => new EntityId<Achievement>(value));

              builder.Property(ua => ua.UnlockedAt)
                     .HasColumnName("unlocked_at")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP")
                     .HasColumnType("timestamp with time zone");

              builder.HasIndex(ua => ua.UserId);
              builder.HasIndex(ua => ua.AchievementId);
       }
}