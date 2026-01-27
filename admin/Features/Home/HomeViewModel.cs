using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.Enums;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace admin.Features.Home;
public partial class HomeViewModel : BaseViewModel, IDisposable
{
    [ObservableProperty]
    private HealthResponse? _currentHealth;

    [ObservableProperty]
    private string _pingValue = "0 мс";

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

    private readonly CancellationTokenSource _cts = new();
    private readonly ICurrentUserService _currentUserService;
    private readonly IApiService _apiService;
    private readonly ILogger<HomeViewModel> _logger;
    private Task? _healthMonitor;
    public HomeViewModel(
        ICurrentUserService currentUserService, 
        IApiService apiService,
        ILogger<HomeViewModel> logger
    )
    {
        _currentUserService = currentUserService;
        _apiService = apiService;
        _logger = logger;

        InitHealthMonitor();
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

    private void InitHealthMonitor()
    {
        _healthMonitor = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    await FetchHealthDataAsync(_cts.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }, _cts.Token);
    }

    private async Task FetchHealthDataAsync(CancellationToken ct = default)
    {
        await HandleActionAsync(async () =>
        {
            CalculatePing(await _apiService.Health.GetPing());
            CurrentHealth = await _apiService.Health.GetDetailed(ct);
        }, false);
    }

    private void CalculatePing(DateTime dt)
    {
        PingValue = $"{DateTime.UtcNow.CompareTo(dt)} мс";
    }
    private string GetUptime()
    {
        if (CurrentHealth == null)
            return "Загрузка";

        int days = (int)CurrentHealth.Uptime.TotalDays;
        int hours = (int)CurrentHealth.Uptime.TotalHours;
        int mins = (int)CurrentHealth.Uptime.TotalMinutes;

        var dayWord = days switch
        {
            0 => "дней",
            1 => "день",
            < 5 => "дня",
            _ => "дней"
        };

        return $"{days} {dayWord}, {(hours < 10 ? "0" + hours : hours)}:{(mins < 10 ? "0" + mins : mins)}";
    }
    private string GetProblems()
    {
        if (CurrentHealth == null)
            return "";

        int counter = -1;

        foreach (var res in Enum.GetValues<UnhealthyReason>())
        {
            if ((CurrentHealth.UnhealthyReasons & res) == res)
                counter++;
        }

        var finalString = counter switch
        {
            0 => "Нет проблем",
            1 => $"{counter} проблема",
            <5 => $"{counter} проблемы",
            _ => $"{counter} проблем"
        };

        return finalString;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}