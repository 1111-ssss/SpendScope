using admin.Core.Abstractions;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace admin.Features.Settings;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appTheme = string.Empty;

    private readonly ICurrentUserService _currentUserService;

    public SettingsViewModel(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [RelayCommand]
    private void SetTheme(string theme)
    {

    }
    [RelayCommand]
    private async Task Logout()
    {
        await _currentUserService.LogoutAsync();
    }
}
