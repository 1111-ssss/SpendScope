using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf.Ui.Appearance;

namespace admin.Features.Metrics;

public partial class MetricsViewModel : BaseViewModel
{
    [ObservableProperty]
    private HealthResponse? _currentHealth;

    private int _pingValue;

    //Plots
    [ObservableProperty]
    private WpfPlot _pingPlot = new();
    private readonly ObservableCollection<double> _pingValues = new();

    public string HealthStatus => CurrentHealth switch
    {
        null => "Загрузка",
        { IsHealthy: true } => "Активен",
        _ => "Имеются проблемы"
    };
    public Brush AccentBrush => CurrentHealth switch
    {
        null => Brushes.DarkRed,
        { IsHealthy: true } => ApplicationAccentColorManager.SystemAccentBrush,
        _ => Brushes.DarkRed
    };
    public string Uptime => GetUptime();
    public string TotalRequests => CurrentHealth switch
    {
        null => "Загрузка",
        _ => CurrentHealth.TotalRequests.ToString()
    };
    public string ActiveConnections => CurrentHealth switch
    {
        null => "Загрузка",
        _ => CurrentHealth.ActiveConnections.ToString()
    };
    public string Problems => GetProblems();

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
        OnPropertyChanged(nameof(ActiveConnections));
        OnPropertyChanged(nameof(Problems));
    }

    private void InitPingPlot()
    {
        PingPlot.Plot.Title("Задержка пакетов (пинг)");
        PingPlot.Plot.YLabel("мс");
        PingPlot.Plot.XLabel("Время");

        var streamer = PingPlot.Plot.Add.DataStreamer(15);

        streamer.LineWidth = 3;
        streamer.MarkerSize = 6;
        streamer.ViewScrollLeft();

        _timer.Tick += (sender, e) =>
        {
            var serverTime = CurrentHealth?.CurrentTime ?? DateTime.UtcNow;
            var ping = DateTime.UtcNow.Subtract(serverTime).TotalMilliseconds;

            streamer.Add(ping);

            PingPlot.Refresh();
        };
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