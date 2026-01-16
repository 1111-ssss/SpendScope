using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace admin.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = [];

    private bool _isLoaded = false;

    public MainWindowViewModel(INavigationService navigationService)
    {
        if (!_isLoaded)
        {
            InitViewModel();
        }
    }
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

        TrayMenuItems = [new() { Header = "Home", Tag = "tray_home" }];

        _isLoaded = true;
    }
}
