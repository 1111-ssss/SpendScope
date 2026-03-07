using admin.Core.Abstractions;
using admin.Core.DTO.Logging.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace admin.Features.Logs;

public partial class LogsViewModel : BaseViewModel
{
    [ObservableProperty]
    private LogListResponse? _logPage;

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int ? _pageSize = 10;

    [ObservableProperty]
    private LogResponse? _selectedLog;

    public string CardText => SelectedLog != null
        ? $"Уровень: {SelectedLog.Level}\nДата: {SelectedLog.Timestamp.ToShortDateString()}\nСообщение: {SelectedLog.Message}\n\nОшибка: {SelectedLog.Exception ?? "Отсутствует"}"
        : "Здесь отобразится информация о журнале";

    public List<LogResponse> LogPageItems => LogPage?.Items ?? new();

    private readonly IApiService _apiService;
    public LogsViewModel(
        IApiService apiService
    )
    {
        _apiService = apiService;
    }
}
