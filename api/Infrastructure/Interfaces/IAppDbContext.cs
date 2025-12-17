using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<AppVersion> AppVersions { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}