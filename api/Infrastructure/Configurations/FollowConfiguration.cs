using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.HasKey(f => new { f.FollowerId, f.FollowedId });

        builder.Property(f => f.FollowedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_follows_not_self", 
            "follower_id <> followed_id"
        ));

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(f => f.FollowerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(f => f.FollowedId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}