namespace admin.Core.Enums;

[Flags]
public enum UnhealthyReason
{
    None = 0,
    Database = 1 << 0,
    HighCpu = 1 << 1,
    HighMemory = 1 << 2,
    HighDisk = 1 << 3,
    DbLatency = 1 << 4,
}