using admin.Features.Home;
using admin.Features.Settings;
using admin.Shell.Views;
using Wpf.Ui.Controls;

namespace admin.Shell.ViewModel;
public partial class MainWindowViewModel
{
    private void InitViewModel()
    {
        CurrentViewModel = _mainContentViewModel;

        ApplicationTitle = "Панель администратора";

        TrayMenuItems = [
            new() { Header = "Главная", Tag = "tray_home" },
            new() { Header = "Настройки", Tag = "tray_settings" },
            new() { Header = "Выйти", Tag = "tray_close" }
        ];

        SetupTrayMenuEvents();

        _isLoaded = true;
    }
    private void InitNavigation()
    {
        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Главная",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(HomeView),
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
                TargetPageType = typeof(SettingsView),
            },
        ];
    }
}