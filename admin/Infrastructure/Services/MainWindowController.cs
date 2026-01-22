using admin.Core.Interfaces;
using admin.Core.Model;
using admin.Shell.ViewModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace admin.Infrastructure.Services;

public class MainWindowController : IMainWindowController
{
    private MainWindowViewModel? _mainVM;
    public MainWindowViewModel MainVM => _mainVM ??
        throw new InvalidOperationException("MainWindowViewModel не был установлен");

    private Dictionary<string, WindowNavigationProps> _windowsDictionary = [];
    private INavigationService _navigationService;

    public MainWindowController(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public void SetMainViewModel(MainWindowViewModel mainVM)
    {
        _mainVM = mainVM;
    }
    public void CreateNewWindow(string name, WindowNavigationProps props)
    {
        if (_windowsDictionary.ContainsKey(name))
            return;

        var anyItemWithNoTag = props.TrayMenuItems.Any(c => string.IsNullOrEmpty(c.Key.Tag.ToString()));
        if (anyItemWithNoTag)
            throw new InvalidOperationException("Список TrayMenuItems содержит MenuItem без свойства Tag");

        _windowsDictionary.Add(name, props);
    }
    public void NavigateToWindow(string name)
    {
        if (!_windowsDictionary.TryGetValue(name, out var props))
            throw new InvalidOperationException($"Не удалось найти окно с названием {name}");

        MainVM.ApplicationTitle = props.ApplicationTitle;
        MainVM.NavigationItems = props.NavigationItems;
        MainVM.NavigationFooter = props.NavigationFooter;

        MainVM.TrayMenuItems.Clear();
        foreach (var (item, action) in props.TrayMenuItems)
        {
            MainVM.TrayMenuItems.Add(item);
            MainVM.TrayActions.Add(item.Tag.ToString()!, action);
        }
        MainVM.SetupTrayMenuEvents();

        if (props.NavigationItems.Count() > 0)
        {
            var item = props.NavigationItems.First() as NavigationViewItem;
            if (item != null && item.TargetPageType != null)
                _navigationService.Navigate(item.TargetPageType);
        }
    }
}