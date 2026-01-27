using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class LogEntry : IAggregateRoot
{
    public EntityId<LogEntry> Id { get; set; }
    public DateTime? Timestamp { get; set; } = DateTime.UtcNow;
    public int Level { get; set; } = 1;
    public string Message { get; set; } = "Сообщение пустое";
    public string MessageTemplate { get; set; } = "Сообщение пустое";
    public string? Exception { get; set; }
    public string Properties { get; set; } = String.Empty;
    public string LogEvent { get; set; } = String.Empty;
    private LogEntry() { }
}