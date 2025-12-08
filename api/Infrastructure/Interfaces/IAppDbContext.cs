using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Profile> Profiles { get; }
        DbSet<Achievement> Achievements { get; }
        DbSet<UserAchievement> UserAchievements { get; }
        DbSet<AppVersion> AppVersions { get; }
        DbSet<Follow> Follows { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}