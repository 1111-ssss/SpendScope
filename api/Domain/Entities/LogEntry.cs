using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class LogEntry : IAggregateRoot
{
    public EntityId<LogEntry> Id { get; set; }
    public DateTime? Timestamp { get; set; } = DateTime.UtcNow;
    public string Level { get; set; } = "Debug";
    public string Message { get; set; } = "Сообщение пустое";
    public string? Exception { get; set; }
    private LogEntry() { }
}