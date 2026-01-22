using admin.Core.Abstractions;
using admin.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui;

namespace admin.Shell.ViewModel;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _applicationTitle = "SpendScope";

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    [ObservableProperty]
    private ObservableCollection<Wpf.Ui.Controls.MenuItem> _trayMenuItems = [];

    private INavigationService _navigationService;
    private ICurrentUserService _currentUserService;
    private IMainWindowController _mainWindowController;

    public MainWindowViewModel(
        INavigationService navigationService,
        ICurrentUserService currentUserService,
        IMainWindowController mainWindowController
    )
    {
        _navigationService = navigationService;
        _currentUserService = currentUserService;
        _mainWindowController = mainWindowController;

        mainWindowController.SetMainViewModel(this);
        InitWindows();

        //if (!_currentUserService.IsAuthenticated)
        //{
        //    _mainWindowController.NavigateToWindow("Auth");
        //    return;
        //}

        //_mainWindowController.NavigateToWindow("Main");

        _currentUserService.UserStateChanged += OnUserStateChanged;
    }

    private void OnUserStateChanged(object? sender, EventArgs e)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            _mainWindowController.NavigateToWindow("Auth");
            return;
        }

        _mainWindowController.NavigateToWindow("Main");
    }
}
