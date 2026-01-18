using admin.Core.Abstractions;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace admin.Features.Auth.Pages;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _identifier = String.Empty;

    [ObservableProperty]
    private string _password = String.Empty;

    private AuthViewModel _parent;
    private IAuthService _authService;
    public LoginViewModel(AuthViewModel parent, IAuthService authService)
    {
        _parent = parent;
        _authService = authService;
    }

    [RelayCommand]
    private void NavigateToRegister() => _parent.NavigateToReigsterCommand.Execute(null);

    [RelayCommand]
    private void NavigateToSettings() => _parent.NavigateToSettingsCommand.Execute(null);

    [RelayCommand]
    private void Login()
    {
        var result = _authService.Login(Identifier, Password);

        throw new NotImplementedException();
    }
}
