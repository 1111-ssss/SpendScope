using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace admin.ViewModels.MainWindow;

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

    private INavigationService _navigationService;
    private bool _isLoaded = false;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        if (!_isLoaded)
            InitViewModel();
    }
}
