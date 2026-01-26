namespace Application.Abstractions.DataBase;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
    Task<long> CalcDBLatencyAsync(CancellationToken ct = default);
}