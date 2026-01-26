using System.Diagnostics;
using Application.Abstractions.Misc;

namespace Infrastructure.Services.Misc;

public class CpuUsageService : ICpuUsageService
{
    private readonly Process _currentProcess;
    private TimeSpan _lastCpuTime;
    private DateTime _lastCheckTime;
    private double _lastCpuPercent;
    public CpuUsageService()
    {
        _currentProcess = Process.GetCurrentProcess();
        _lastCpuTime = _currentProcess.TotalProcessorTime;
        _lastCheckTime = DateTime.UtcNow;
        _lastCpuPercent = 0;
    }
    public double GetCpuUsagePercent()
    {
        var now = DateTime.UtcNow;
        var cpuTime = _currentProcess.TotalProcessorTime;

        var timeDiff = (now - _lastCheckTime).TotalMilliseconds;
        if (timeDiff < 5000) return _lastCpuPercent;

        var cpuDiff = (cpuTime - _lastCpuTime).TotalMilliseconds;
        var cpuPercent = (cpuDiff / timeDiff) * 100 * Environment.ProcessorCount;

        _lastCpuTime = cpuTime;
        _lastCheckTime = now;
        _lastCpuPercent = Math.Min(100, Math.Max(0, cpuPercent));

        return Math.Round(_lastCpuPercent, 1);
    }
}