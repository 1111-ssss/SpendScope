using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Features.Settings;
using System.Security.RightsManagement;
using Wpf.Ui.Controls;

namespace admin.Shell.ViewModel;
public partial class MainWindowViewModel
{
    public void InitNavigation()
    {
        switch (CurrentWindow)
        {
            case "MainWindow":
                _navigationService.Navigate(typeof(HomePage));
                break;
            case "AuthWindow":
                _navigationService.Navigate(typeof(AuthLoginPage));
                break;
            default:
                _navigationService.Navigate(typeof(HomePage));
                break;
        }
    }
    private void InitMainViewModel()
    {
        CurrentWindow = "MainWindow";

        ApplicationTitle = "SpendScope - Панель администратора";

        TrayMenuItems = [
            new() { Header = "Главная", Tag = "tray_home" },
            new() { Header = "Настройки", Tag = "tray_settings" },
            new() { Header = "Выйти", Tag = "tray_close" }
        ];

        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Главная",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(HomePage),
            },
            //new NavigationViewItem()
            //{
            //    Content = "Пользователи",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            //    TargetPageType = typeof(UsersView),
            //},
        ];

        NavigationFooter =
        [
            new NavigationViewItem()
            {
                Content = "Настройки",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage),
            },
        ];

        SetupTrayMenuEvents();

        _isLoaded = true;
    }
    private void InitAuthViewModel()
    {
        CurrentWindow = "AuthWindow";

        ApplicationTitle = "SpendScope - Авторизация";

        TrayMenuItems = [
            new() { Header = "Вход", Tag = "tray_auth_login" },
            new() { Header = "Регистрация", Tag = "tray_auth_register" },
            new() { Header = "Настройки", Tag = "tray_auth_settings" },
            new() { Header = "Выйти", Tag = "tray_close" },
        ];

        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Вход",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(AuthLoginPage),
            },
            new NavigationViewItem()
            {
                Content = "Регистрация",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(AuthRegisterPage),
            },
        ];

        NavigationFooter =
        [
            new NavigationViewItem()
            {
                Content = "Настройки",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(AuthSettingsPage),
            },
        ];

        SetupTrayMenuEvents();

        _isLoaded = true;
    }
}