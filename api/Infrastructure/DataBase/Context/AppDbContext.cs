using Application.Abstractions.DataBase;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Context;

public partial class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Achievement> Achievements { get; set; }
    public virtual DbSet<AppVersion> AppVersions { get; set; }
    public virtual DbSet<Follow> Follows { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserAchievement> UserAchievements { get; set; }
    public virtual DbSet<LogEntry> LogEntries { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public async Task<int> GetTrackedEntitiesCountAsync()
    {
        return ChangeTracker.Entries().Count();
    }
}