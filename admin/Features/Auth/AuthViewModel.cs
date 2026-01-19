using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Shell.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;

namespace admin.Features.Auth.Pages;

public partial class AuthViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _identifier = String.Empty;

    [ObservableProperty]
    private string _email = String.Empty;

    [ObservableProperty]
    private string _password = String.Empty;

    [ObservableProperty]
    private string _serverUrl = String.Empty;

    [ObservableProperty]
    private bool _rememberCredentials = true;

    private IAuthService _authService;
    private INavigationService _navigationService;
    public AuthViewModel()
    {
        _authService = App.GetRequiredService<IAuthService>();
        _navigationService = App.GetRequiredService<INavigationService>();
    }

    [RelayCommand]
    private void NavigateToLogin() => _navigationService.Navigate(typeof(AuthLoginPage));
    [RelayCommand]
    private void NavigateToRegister() => _navigationService.Navigate(typeof(AuthRegisterPage));

    [RelayCommand]
    private void NavigateToSettings() => _navigationService.Navigate(typeof(AuthSettingsPage));

    [RelayCommand]
    private void Login()
    {
        //var result = _authService.Login(Identifier, Password);

        //throw new NotImplementedException();
        var mainVM = App.GetRequiredService<MainWindowViewModel>();

        mainVM.NavigateToMainWindow();
    }
    [RelayCommand]
    private void Register()
    {
        var result = _authService.Register(Identifier, Email, Password);

        throw new NotImplementedException();
    }
    [RelayCommand]
    private void ChangeSettings()
    {
        //var result = _authService.ChangeSettings(_serverUrl, _rememberCredentials);

        throw new NotImplementedException();
    }
}
