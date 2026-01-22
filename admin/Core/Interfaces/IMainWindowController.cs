using admin.Core.Model;
using admin.Shell.ViewModel;

namespace admin.Core.Interfaces;

public interface IMainWindowController
{
    public void CreateNewWindow(string name, WindowNavigationProps props);
    public void NavigateToWindow(string name);
    public void SetMainViewModel(MainWindowViewModel mainVM);
}
