using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;

namespace admin.Features.Home;
public partial class HomeViewModel : BaseViewModel
{
    [ObservableProperty]
    private HealthResponse? _currentHealth;

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
}