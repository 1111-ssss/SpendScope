using admin.Core.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using ScottPlot.WPF;
using System.Collections.ObjectModel;

namespace admin.Features.Metrics;

public partial class MetricsViewModel : BaseViewModel
{
    //Plot vars
    [ObservableProperty]
    private WpfPlot _pingPlot = new();
    private readonly ObservableCollection<double> _pingValues = new();

    [ObservableProperty]
    private WpfPlot _dbLatencyPlot = new();
    private readonly ObservableCollection<double> _dbLatencyValues = new();

    [ObservableProperty]
    private WpfPlot _memoryUsagePlot = new();
    private readonly ObservableCollection<double> _memoryUsageValues = new();

    //Plot inits
    private void InitPingPlot()
    {
        PingPlot.Plot.Title("Задержка пакетов (пинг)", 14);
        PingPlot.Plot.YLabel("мс", 12);
        PingPlot.Plot.XLabel("Время", 12);

        var streamer = PingPlot.Plot.Add.DataStreamer(10);

        streamer.LineWidth = 3;
        streamer.MarkerSize = 6;
        streamer.ViewScrollLeft();
        streamer.Period = 10;

        _timer.Tick += (sender, e) =>
        {
            var serverTime = CurrentHealth?.CurrentTime ?? DateTime.UtcNow;
            var ping = DateTime.UtcNow.Subtract(serverTime).TotalMilliseconds;

            streamer.Add(ping);

            PingPlot.Refresh();
        };
    }

    private void InitDbLatencyPlot()
    {
        DbLatencyPlot.Plot.Title("Задержка базы данных", 14);
        DbLatencyPlot.Plot.YLabel("мс", 12);
        DbLatencyPlot.Plot.XLabel("Время", 12);

        var streamer = DbLatencyPlot.Plot.Add.DataStreamer(10);

        streamer.LineWidth = 3;
        streamer.MarkerSize = 6;
        streamer.ViewScrollLeft();
        streamer.Period = 10;

        _timer.Tick += (sender, e) =>
        {
            var latencyMs = CurrentHealth?.DbLatency ?? 0;

            streamer.Add(latencyMs);

            DbLatencyPlot.Refresh();
        };
    }

    private void InitMemoryUsagePlot()
    {
        MemoryUsagePlot.Plot.Title("Использование RAM", 14);
        MemoryUsagePlot.Plot.YLabel("МБ", 12);
        MemoryUsagePlot.Plot.XLabel("Время", 12);

        var streamer = MemoryUsagePlot.Plot.Add.DataStreamer(10);

        streamer.LineWidth = 3;
        streamer.MarkerSize = 6;
        streamer.ViewScrollLeft();
        streamer.Period = 10;

        _timer.Tick += (sender, e) =>
        {
            var memoryUsageMB = (int?)CurrentHealth?.MemoryUsage ?? 0;

            streamer.Add(memoryUsageMB);

            MemoryUsagePlot.Refresh();
        };
    }
}