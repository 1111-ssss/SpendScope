using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.Enums;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf.Ui.Appearance;

namespace admin.Features.Home;
public partial class HomeViewModel : BaseViewModel, IDisposable
{
    [ObservableProperty]
    private HealthResponse? _currentHealth;

    public string PingValue => CalculatePing();
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

    private readonly ICurrentUserService _currentUserService;
    private readonly IApiService _apiService;
    private readonly ILogger<HomeViewModel> _logger;
    public HomeViewModel(
        ICurrentUserService currentUserService, 
        IApiService apiService,
        ILogger<HomeViewModel> logger
    )
    {
        _currentUserService = currentUserService;
        _apiService = apiService;
        _logger = logger;

        InitTimer();
    }
    private void InitTimer()
    {
        _ = FetchHealthDataAsync();
        _timer.Interval = TimeSpan.FromSeconds(5);
        _timer.Tick += async (sender, e) => await FetchHealthDataAsync();
    }

    partial void OnCurrentHealthChanged(HealthResponse? value)
    {
        OnPropertyChanged(nameof(HealthStatus));
        OnPropertyChanged(nameof(AccentBrush));
        OnPropertyChanged(nameof(Uptime));
        OnPropertyChanged(nameof(TotalRequests));
        OnPropertyChanged(nameof(ActiveConnections));
        OnPropertyChanged(nameof(Problems));
        OnPropertyChanged(nameof(PingValue));
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

    private string CalculatePing()
    {
        var serverTime = CurrentHealth?.CurrentTime ?? DateTime.UtcNow;
        return $"{(int)DateTime.UtcNow.Subtract(serverTime).TotalMilliseconds} мс";
    }
    private string GetUptime()
    {
        if (CurrentHealth == null)
            return "Загрузка";

        int days = (int)CurrentHealth.Uptime.TotalDays;
        int hours = (int)CurrentHealth.Uptime.TotalHours % 24;
        int mins = (int)CurrentHealth.Uptime.TotalMinutes % 60;

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