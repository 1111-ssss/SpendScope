using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        builder.HasKey(ua => new { ua.UserId, ua.AchievementId });

        builder.Property(ua => ua.UnlockedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}