using admin.Core.Abstractions;
using admin.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace admin.Features.Versions;

public partial class VersionsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<VersionsViewModel> _logger;

    public VersionsViewModel(
        IApiService apiService,
        ICurrentUserService currentUserService,
        ILogger<VersionsViewModel> logger
    )
    {
        _apiService = apiService;
        _currentUserService = currentUserService;
        _logger = logger;
    }
}
