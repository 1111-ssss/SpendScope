using admin.Core.Model;
using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Features.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace admin.Shell.ViewModel;
public partial class MainWindowViewModel
{
    public void InitWindows1()
    {
        //MainWindow
        _mainWindowController.CreateNewWindow("Main", new WindowNavigationProps
        {
            ApplicationTitle = "SpendScope - Панель администратора",

            NavigationItems =
            [
                new NavigationViewItem()
                {
                    Content = "Главная",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                    TargetPageType = typeof(HomePage),
                }
                //new NavigationViewItem()
                //{
                //    Content = "Пользователи",
                //    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                //    TargetPageType = typeof(UsersView),
                //},
            ],

            NavigationFooter =
            [
                new NavigationViewItem()
                {
                    Content = "Настройки",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(SettingsPage),
                },
            ],

            TrayMenuItems = {
                {
                    new() { Header = "Главная", Tag = "tray_home" },
                    () => _navigationService.Navigate(typeof(HomePage))
                },
                {
                    new() { Header = "Настройки", Tag = "tray_settings" },
                    () => _navigationService.Navigate(typeof(SettingsPage))
                },
                {
                    new() { Header = "Выйти", Tag = "tray_close" },
                    () => App.Current.Shutdown()
                },
            },
        });

        //AuthWindow
        _mainWindowController.CreateNewWindow("Auth", new WindowNavigationProps
        {
            ApplicationTitle = "SpendScope - Авторизация",

            NavigationItems =
            [
                new NavigationViewItem()
                {
                    Content = "Вход",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.ArrowForwardDownPerson24 },
                    TargetPageType = typeof(AuthLoginPage),
                },
                new NavigationViewItem()
                {
                    Content = "Регистрация",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.PersonAdd24 },
                    TargetPageType = typeof(AuthRegisterPage),
                },
            ],

            NavigationFooter =
            [
                new NavigationViewItem()
                {
                    Content = "Настройки",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(AuthSettingsPage),
                },
            ],

            TrayMenuItems = {
                {
                    new() { Header = "Вход", Tag = "tray_auth_login" },
                    () =>  _navigationService.Navigate(typeof(AuthLoginPage))
                },
                {
                    new() { Header = "Регистрация", Tag = "tray_auth_register" },
                    () => _navigationService.Navigate(typeof(AuthRegisterPage))
                },
                {
                    new() { Header = "Настройки", Tag = "tray_auth_settings" },
                    () => _navigationService.Navigate(typeof(AuthSettingsPage))
                },
                {
                    new() { Header = "Выйти", Tag = "tray_close" },
                    () => App.Current.Shutdown()
                },
            },
        });
        _isLoaded = true;
        OnUserStateChanged(this, EventArgs.Empty);
    }
}