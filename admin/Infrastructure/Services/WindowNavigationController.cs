using admin.Core.Interfaces;
using admin.Shell.ViewModel;

namespace admin.Infrastructure.Services;

public class WindowNavigationController : IWindowNavigationController
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    public WindowNavigationController(MainWindowViewModel viewModel)
    {
        _mainWindowViewModel = viewModel;
    }
    public void NavigateToMainWindow() => _mainWindowViewModel.NavigateToMainWindow();
    public void NavigateToAuthWindow() => _mainWindowViewModel.NavigateToAuthWindow();
}
