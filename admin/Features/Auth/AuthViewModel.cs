using admin.Core.Abstractions;
using admin.Features.Auth.Pages;
using admin.Shell.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace admin.Features.Auth;

public partial class AuthViewModel : BaseViewModel
{
    [ObservableProperty]
    private object _currentPage;

    private LoginViewModel _loginVM;
    private RegisterViewModel _registerVM;
    private AuthSettingsViewModel _settingsVM;

    //private MainWindowViewModel _mainWindowVM;
    private readonly Action _onSuccessfulLogin;
    public AuthViewModel(
        //MainWindowViewModel mainViewModel,
        Action onSuccessfulLogin,
        LoginViewModel loginVM,
        RegisterViewModel registerVM,
        AuthSettingsViewModel settingsVM
    )
    {
        CurrentPage = loginVM;

        //_mainWindowVM = mainViewModel;
        _onSuccessfulLogin = onSuccessfulLogin;
        _loginVM = loginVM;
        _registerVM = registerVM;
        _settingsVM = settingsVM;
    }

    [RelayCommand]
    private void NavigateToLogin() => CurrentPage = _loginVM;

    [RelayCommand]
    private void NavigateToReigster() => CurrentPage = _registerVM;

    [RelayCommand]
    private void NavigateToSettings() => CurrentPage = _settingsVM;

    //[RelayCommand]
    //private void NavigateToMainWindow() => _mainWindowVM.NavigateToMainWindow();

    [RelayCommand]
    private void OnSuccessfulLogin() => _onSuccessfulLogin.Invoke();
}
