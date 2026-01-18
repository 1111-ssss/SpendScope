using admin.Core.Abstractions;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace admin.Features.Auth.Pages;

public partial class RegisterViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _username = String.Empty;

    [ObservableProperty]
    private string _email = String.Empty;

    [ObservableProperty]
    private string _password = String.Empty;

    private AuthViewModel _parent;
    private IAuthService _authService;
    public RegisterViewModel(AuthViewModel parent, IAuthService authService)
    {
        _parent = parent;
        _authService = authService;
    }

    [RelayCommand]
    private void NavigateToLogin() => _parent.NavigateToLoginCommand.Execute(null);

    [RelayCommand]
    private void NavigateToSettings() => _parent.NavigateToSettingsCommand.Execute(null);

    [RelayCommand]
    private void Register()
    {
        var result = _authService.Register(Username, Email, Password);

        throw new NotImplementedException();
    }
}
