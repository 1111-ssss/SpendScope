using admin.Core.Abstractions;
using admin.Core.Interfaces;
using admin.Core.Model;
using admin.Core.DTO.Auth.Requests;
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
    private ApplicationSettings _settings;

    private IApiService _apiService;
    private ICurrentUserService _currentUserService;
    private INavigationService _navigationService;
    private IMainWindowController _windowNavigationController;

    public AuthViewModel(
        IApiService apiService,
        INavigationService navigationService,
        ICurrentUserService currentUserService,
        IMainWindowController windowNavigationController,
        IAppSettingsService appSettingsService
    )
    {
        _apiService = apiService;
        _currentUserService = currentUserService;
        _navigationService = navigationService;
        _windowNavigationController = windowNavigationController;

        Settings = appSettingsService.Current;

        if (Settings.RememberUsername && !string.IsNullOrEmpty(Settings.SavedUsername))
            Identifier = Settings.SavedUsername;

        _ = HandleActionAsync(async () =>
        {
            await _apiService.Health.GetHealth();
        }, true, "Нет ответа от сервера", "Проверьте подключение к интернету или поменяйте адрес удаленного сервера");
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
        Settings.SavedUsername = Identifier;
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
        Settings.SavedUsername = Identifier;
        await HandleActionAsync(async () =>
        {
            var result = await _apiService.Auth.Register(
                new RegisterRequest(Identifier, Email, Password)
            );

            await _currentUserService.LoginAsync(result);
            _windowNavigationController.NavigateToWindow("Main");
        });
    }
}