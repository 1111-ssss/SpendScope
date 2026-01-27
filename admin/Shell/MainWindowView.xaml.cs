using admin.Core.Interfaces;
using admin.Shell.ViewModel;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace admin.Shell;

public partial class MainWindowView : FluentWindow, INavigationWindow
{
    public MainWindowViewModel ViewModel { get; }

    private bool _paneClosedByUser = false;
    private bool _paneTriggeredByCode = false;

    public MainWindowView(
        MainWindowViewModel viewModel,
        INavigationService navigationService,
        IContentDialogService contentDialogService,
        IAppSettingsService appSettingsService
    )
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        SystemThemeWatcher.Watch(this);
        appSettingsService.UpdateTheme();

        InitializeComponent();

        navigationService.SetNavigationControl(RootNavigation);
        contentDialogService.SetDialogHost(RootDialogHost);
    }
    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
        RootNavigation.SetPageProviderService(navigationViewPageProvider);

    public void ShowWindow() {
        Show();

        ViewModel.InitWindows();
    }

    public void CloseWindow() => Close();

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        Application.Current.Shutdown();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
    private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_paneClosedByUser)
        {
            return;
        }

        _paneTriggeredByCode = true;
        RootNavigation.SetCurrentValue(
            NavigationView.IsPaneOpenProperty,
            e.NewSize.Width > 1200
        );
        _paneTriggeredByCode = false;
    }
    private void RootNavigation_SelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not NavigationView navigationView)
        {
            return;
        }

        var visibility = ViewModel.ApplicationWindow == "Main"
            ? Visibility.Visible
            : Visibility.Collapsed;

        RootNavigation.SetCurrentValue(
            NavigationView.HeaderVisibilityProperty,
            visibility
        );
    }
    private void RootNavigation_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
    {
        if (_paneTriggeredByCode)
        {
            return;
        }

        _paneClosedByUser = false;
    }

    private void RootNavigation_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
    {
        if (_paneTriggeredByCode)
        {
            return;
        }

        _paneClosedByUser = true;
    }
}