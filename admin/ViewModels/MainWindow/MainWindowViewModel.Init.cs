using Wpf.Ui.Controls;

namespace admin.ViewModels.MainWindow;
public partial class MainWindowViewModel
{
    private void InitViewModel()
    {
        ApplicationTitle = "Панель администратора";

        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Главная",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.HomeView),
            },
            //new NavigationViewItem()
            //{
            //    Content = "Пользователи",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            //    TargetPageType = typeof(Views.Pages.UsersView),
            //},
        ];

        NavigationFooter =
        [
            new NavigationViewItem()
            {
                Content = "Настройки",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsView),
            },
        ];

        TrayMenuItems = [
            new() { Header = "Главная", Tag = "tray_home" },
            new() { Header = "Настройки", Tag = "tray_settings" },
            new() { Header = "Выйти", Tag = "tray_close" }
        ];

        SetupTrayMenuEvents();

        _isLoaded = true;
    }
}