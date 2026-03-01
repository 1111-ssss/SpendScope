using admin.Core.Abstractions;
using admin.Core.DTO.Health.Responses;
using admin.Core.DTO.Versions.Responses;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace admin.Features.Versions;

public partial class VersionsViewModel : BaseViewModel
{
    public readonly List<string> Branches = new() { "Stable", "Dev" };
    public List<AppVersionResponse>? Versions => AllVersions?.Versions[SelectedBranch];

    [ObservableProperty]
    private string _selectedBranch = "Stable";

    [ObservableProperty]
    private AppVersionResponse? _selectedVersion;

    public string? Build => SelectedVersion?.Build != null
        ? $"Билд версии: {SelectedVersion?.Build.ToString()}"
        : string.Empty;
    public string? UploadedAt => SelectedVersion?.UploadedAt != null
        ? $"Загружена: {SelectedVersion?.UploadedAt.Value.ToShortTimeString()}"
        : string.Empty;

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

    public override void OnNavigatedTo()
    {
        base.OnNavigatedTo();

        _ = FetchAllVersions();
    }

    partial void OnSelectedBranchChanged(string value)
    {
        OnPropertyChanged(nameof(Versions));
    }

    partial void OnSelectedVersionChanged(AppVersionResponse? value)
    {
        OnPropertyChanged(nameof(Build));
        OnPropertyChanged(nameof(UploadedAt));
    }

    public async Task FetchAllVersions()
    {
        await HandleActionAsync(async () =>
        {
            AllVersions = await _apiService.Versions.GetAllVersions();
            OnPropertyChanged(nameof(Versions));
        });
    }

    [RelayCommand]
    private async Task RemoveVersion()
    {
        await HandleActionAsync(async () =>
        {
            if (SelectedVersion?.Branch == null)
                return;

            await _apiService.Versions.DeleteVersion(SelectedVersion.Branch, SelectedVersion.Build.ToString());

            OnPropertyChanged(nameof(Versions));
            OnPropertyChanged(nameof(SelectedVersion));
        });
    }
}
