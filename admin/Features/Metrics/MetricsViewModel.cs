using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;

namespace admin.Features.Metrics;

public partial class MetricsViewModel : BaseViewModel
{
    [ObservableProperty]
    private HealthResponse? _currentHealth;

    private CancellationTokenSource _cts = new();
    private DispatcherTimer _timer = new();

    private readonly IApiService _apiService;
    private readonly ILogger<MetricsViewModel> _logger;
    public MetricsViewModel(
        IApiService apiService,
        ILogger<MetricsViewModel> logger
    )
    {
        _apiService = apiService;
        _logger = logger;

        InitTimerAndPlots();
    }
    private void InitTimerAndPlots()
    {
        _ = FetchHealthDataAsync();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += async (sender, e) => await FetchHealthDataAsync();

        InitPingPlot();
        InitDbLatencyPlot();
        InitMemoryUsagePlot();
    }
    public override void OnNavigatedTo()
    {
        base.OnNavigatedTo();

        _cts = new();
        _timer.Start();
    }
    public override void OnNavigatedFrom()
    {
        base.OnNavigatedFrom();

        _timer.Stop();
        _cts.Cancel();
    }

    partial void OnCurrentHealthChanged(HealthResponse? value)
    {
        OnPropertyChanged(nameof(HealthStatus));
        OnPropertyChanged(nameof(AccentBrush));
        OnPropertyChanged(nameof(Uptime));
        OnPropertyChanged(nameof(TotalRequests));
        OnPropertyChanged(nameof(FailedRequests));
        OnPropertyChanged(nameof(Problems));
        OnPropertyChanged(nameof(CpuUsage));
        OnPropertyChanged(nameof(CpuUsageText));
        OnPropertyChanged(nameof(DiskUsage));
    }

    private async Task FetchHealthDataAsync()
    {
        try
        {
            await HandleActionAsync(async () =>
            {
                CurrentHealth = await _apiService.Health.GetDetailed(_cts.Token);
            }, false);
        }
        catch (TaskCanceledException) when (_cts.IsCancellationRequested)
        {
            //skip
        }
    }
}