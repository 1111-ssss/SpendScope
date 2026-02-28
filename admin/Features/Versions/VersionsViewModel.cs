using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.DTO.Versions.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace admin.Features.Versions;

public partial class VersionsViewModel : BaseViewModel
{
    public readonly List<string> Branches = new () { "Stable", "Dev" };
    public List<AppVersionResponse>? Versions => AllVersions?.Versions[SelectedBranch];

    [ObservableProperty]
    private string _selectedBranch = "Stable";

    private AllVersionsResponse? AllVersions;
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

    partial void OnSelectedBranchChanged(string value)
    {
        OnPropertyChanged(nameof(Versions));
    }

    public async Task FetchAllVersions()
    {
        await HandleActionAsync(async () =>
        {
            AllVersions = await _apiService.Versions.GetAllVersions();
            OnPropertyChanged(nameof(Versions));
        });
    }
}
