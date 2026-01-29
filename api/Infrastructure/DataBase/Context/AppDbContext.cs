using System.Diagnostics;
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
    public virtual DbSet<Expense> Expenses { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
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

    public async Task<bool> CanConnectAsync(CancellationToken ct = default)
    {
        return await Database.CanConnectAsync(ct);
    }
    public async Task<long> CalcDBLatencyAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            // await Database.ExecuteSqlRawAsync("SELECT 1", ct);
            await Database.CanConnectAsync(ct);
            return sw.ElapsedMilliseconds;
        }
        catch
        {
            return -1;
        }
    }
}