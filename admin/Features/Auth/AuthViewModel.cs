using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Features.Auth.DTO.Requests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
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

    private IApiService _apiService;
    private ICurrentUserService _currentUserService;
    private INavigationService _navigationService;
    private IMainWindowController _windowNavigationController;

    public AuthViewModel(
        IApiService apiService,
        INavigationService navigationService,
        ICurrentUserService currentUserService,
        IMainWindowController windowNavigationController
    )
    {
        _apiService = apiService;
        _currentUserService = currentUserService;
        _navigationService = navigationService;
        _windowNavigationController = windowNavigationController;
    }

    [RelayCommand]
    private void NavigateToLogin() => _navigationService.Navigate(typeof(AuthLoginPage));
    [RelayCommand]
    private void NavigateToRegister() => _navigationService.Navigate(typeof(AuthRegisterPage));

    [RelayCommand]
    private void NavigateToSettings() => _navigationService.Navigate(typeof(AuthSettingsPage));

    [RelayCommand]
    private async Task Login()
    {
        await HandleActionAsync(async () =>
        {
            var result = await _apiService.Auth.Login(
                new LoginRequest(Identifier, Password)
            );

            await _currentUserService.LoginAsync(result);
            _windowNavigationController.NavigateToWindow("Main");
        });
    }
    [RelayCommand]
    private async Task Register()
    {
        await HandleActionAsync(async () =>
        {
            var result = await _apiService.Auth.Register(
                new RegisterRequest(Identifier, Email, Password)
            );

            await _currentUserService.LoginAsync(result);
            _windowNavigationController.NavigateToWindow("Main");
        });
    }
    [RelayCommand]
    private void ChangeSettings()
    {
        //var result = _authService.ChangeSettings(_serverUrl, _rememberCredentials);

        throw new NotImplementedException();
    }
}
