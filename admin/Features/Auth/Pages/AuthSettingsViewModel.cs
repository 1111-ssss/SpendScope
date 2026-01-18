using admin.Core.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace admin.Features.Auth.Pages;

public partial class AuthSettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _serverUrl = String.Empty;

    [ObservableProperty]
    private bool _rememberCredentials = true;

    private AuthViewModel _parent;
    public AuthSettingsViewModel(AuthViewModel parent)
    {
        _parent = parent;
    }

    [RelayCommand]
    private void NavigateToLogin() => _parent.NavigateToLoginCommand.Execute(null);

}
