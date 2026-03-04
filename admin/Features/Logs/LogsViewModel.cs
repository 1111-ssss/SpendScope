using admin.Core.Abstractions;
using admin.Core.Interfaces;

namespace admin.Features.Logs;

public class LogsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    public LogsViewModel(
        IApiService apiService
    )
    {
        _apiService = apiService;
    }
}
