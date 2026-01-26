using System.Diagnostics;
using Application.Abstractions.DataBase;
using Infrastructure.DataBase.Context;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public UnitOfWork(AppDbContext courceDbContext)
    {
        _db = courceDbContext;
    }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _db.SaveChangesAsync(ct);
    }
    public async Task<bool> CanConnectAsync(CancellationToken ct = default)
    {
        return await _db.Database.CanConnectAsync(ct);
    }
    public async Task<long> CalcDBLatencyAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _db.Database.CanConnectAsync(ct);
            return sw.ElapsedMilliseconds;
        }
        catch
        {
            return -1;
        }
    }
    public void Dispose()
    {
        _db.Dispose();
    }
}