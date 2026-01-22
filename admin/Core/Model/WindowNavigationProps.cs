using System.Collections.ObjectModel;

namespace admin.Core.Model;

public class WindowNavigationProps
{
    public string ApplicationTitle = "SpendScope";
    public ObservableCollection<object> NavigationItems = [];
    public ObservableCollection<object> NavigationFooter = [];
    public Dictionary<Wpf.Ui.Controls.MenuItem, Action> TrayMenuItems = [];
}
