using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Core.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace admin.Features.Settings;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appTheme = string.Empty;

    [ObservableProperty]
    private ApplicationSettings _settings;

    private readonly ICurrentUserService _currentUserService;
    private readonly IAppSettingsService _appSettingsService;

    public SettingsViewModel(
        ICurrentUserService currentUserService,
        IAppSettingsService appSettings
    )
    {
        _currentUserService = currentUserService;
        _appSettingsService = appSettings;
        Settings = appSettings.Current;
    }

    [RelayCommand]
    private void Close()
    {
        App.Current.Shutdown();
    }
    [RelayCommand]
    private async Task Logout()
    {
        await _currentUserService.LogoutAsync();
    }
}
