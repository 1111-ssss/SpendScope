using admin.Core.Abstractions;
using admin.Core.DTO.Logging.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace admin.Features.Logs;

public partial class LogsViewModel : BaseViewModel
{
    [ObservableProperty]
    public ObservableCollection<LogResponse?> _logResponses = new();

    [ObservableProperty]
    private LogListResponse? _logPage;

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int _pageSize = 10;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private LogResponse? _selectedLog;

    [ObservableProperty]
    private string _level = "Warning";

    public List<string> LogLevels = new() {
        "Trace",
        "Debug",
        "Information",
        "Warning",
        "Error",
        "Critical"
    };

    [ObservableProperty]
    private string _selectedSort = "Timestamp";

    public List<string> Sorting = new() {
        "Timestamp",
        "Level"
    };

    public string CardText => SelectedLog != null
        ? $"Уровень: {SelectedLog.Level}\nДата: {SelectedLog.Timestamp.ToShortDateString()}\nСообщение: {SelectedLog.Message}\n\nОшибка: {SelectedLog.Exception ?? "Отсутствует"}"
        : "Здесь отобразится информация о журнале";

    public List<LogResponse> LogPageItems => LogPage?.Items ?? new();
    private bool _isDesc = true;
    private bool _useMinimalLevel = false;

    private readonly IApiService _apiService;
    public LogsViewModel(
        IApiService apiService
    )
    {
        _apiService = apiService;
    }

    partial void OnSelectedLogChanged(LogResponse? value)
    {
        OnPropertyChanged(nameof(CardText));
    }

    [RelayCommand]
    private async Task FetchLogs()
    {
        await HandleActionAsync(async () =>
        {
            if (!Enum.TryParse<LogLevel>(Level, out LogLevel level))
                return;

            var result = await _apiService.Logging.Get(
                _useMinimalLevel ? null : (int)level,
                (int)level,
                CurrentPage,
                PageSize,
                SelectedSort,
                _isDesc,
                string.IsNullOrEmpty(SearchText) ? null : SearchText
            );
            LogResponses = new ObservableCollection<LogResponse?>(result.Items.ToArray());
        }, true);
    }

    [RelayCommand]
    private async Task ClearLogs()
    {
        await HandleActionAsync(async () =>
        {
            await _apiService.Logging.ClearLogs();
        }, true);
    }

    [RelayCommand]
    private void SwitchMinimalLevel()
    {
        _useMinimalLevel = !_useMinimalLevel;
    }

    [RelayCommand]
    private void SwitchAscSorting()
    {
        _isDesc = !_isDesc;
    }
}
